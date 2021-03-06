﻿using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Entity.Concrete;
using AVBS.Enum;
using AVBS.Infrastructure;
using AVBS.Office.UI.App_Data;
using AVBS.Office.UI.Controllers.Base;
using AVBS.Office.UI.Helper;
using AVBS.Office.UI.Models.User;

namespace AVBS.Office.UI.Controllers {
    [Authentication]
    [AuthorizeRoles( Role.YONETICI_AVUKAT, Role.AVUKAT )]
    public class ClientController : OfficeController< AVBS_Client > {

        public override void ArrangeViewBag ( int id = 0 ) {
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create ( ClientModel model ) {
            if ( !ModelState.IsValid || 
                UserHelper.PasswordCheck( model.Password, model.PasswordRepeat )
                 ) {
                model.Password = "";
                model.PasswordRepeat = "";
                ViewBag.DbSuccess = false;
                ArrangeViewBag();
                return View( model );
            }

            if ( _businessService.GetAllQueryable( x => x.Identity == model.Identity ).Any( ) ) {
                model.Password = "";
                model.PasswordRepeat = "";
                ViewBag.DbSuccess = false;
                ViewBag.DbMessage = "Belirtilen Kimlik Numarası ile bir kayıt bulunmakta.";
                ArrangeViewBag();
                return View( model );
            }

            var entity = ModelOperations( model );
            entity.Password = Md5Hash.CreateMD5( model.Password ).ToLower( );

            if ( !_businessService.Add( entity ) ) {
                ViewBag.DbSuccess = false;
                ArrangeViewBag();
                return View( model );
            }

            TempData["DbSuccess"] = true;

            if ( TempData["ReturnId"] != null ) return RedirectToAction( "Index", new { id = TempData["ReturnId"] } );
            return RedirectToAction( "Index" );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual JsonResult CreateWithReturn ( ClientModel model ) {
            if ( !ModelState.IsValid ||
                UserHelper.PasswordCheck( model.Password, model.PasswordRepeat )||
                _businessService.GetAllQueryable( x => x.Identity == model.Identity ).Any() 
                 ) {
                return Json( new { error = 1, message = "Error" } );
            }

            var entity = ModelOperations( model );
            entity.Password = Md5Hash.CreateMD5( model.Password ).ToLower();

            ModelOperations( ref entity );

            entity = _businessService.AddWithReturnEntity( entity );

            if ( entity.Id == 0 ) {
                return Json( new { error = 1, message = "Error" } );
            }

            var returnText = entity.Name + " " + entity.Surname ;
            return Json( new { error = 0, value = new { id = entity.Id, text = returnText } } );
        }


        public override ActionResult Edit ( int id ) {
            ArrangeViewBag( id );
            EditSpecifications( id );
            AVBS_Client entity = _businessService.GetAsNoTracking( id );
            var model = ConvertToModel( entity );

            return View( model );
        }
        public override ActionResult EditPartial ( int id ) {
            ArrangeViewBag( id );
            EditSpecifications( id );
            AVBS_Client entity = _businessService.GetAsNoTracking( id );
            var model = ConvertToModel( entity );

            return PartialView( model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit ( ClientModel model ) {
            if ( !ModelState.IsValid ) {
                ViewBag.DbSuccess = false;
                ArrangeViewBag( model.Id );
                EditSpecifications( model.Id );
                return View( model );
            }

            var entity = _businessService.Get( model.Id );
            model.Identity = entity.Identity;

            entity = ModelOperations( model, entity );
            if ( !_businessService.Update( entity ) ) {
                ViewBag.DbSuccess = false;
                entity = _businessService.Get( entity.Id );
                ArrangeViewBag( entity.Id );
                EditSpecifications( entity.Id );
                return View( entity );
            }

            TempData["DbSuccess"] = true;
            if ( TempData["ReturnId"] != null ) return RedirectToAction( "Index", new { id = TempData["ReturnId"] } );
            return RedirectToAction( "Index" );
        }

        public ActionResult RefreshPasswordPartial ( int id ) {
            var entity = _businessService.Get( id );
            PasswordModel model = new PasswordModel(  ) {
                UserId = id,
                UserFullName = entity.Name + " " + entity.Surname
            };
            return PartialView( "RefreshPassword", model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RefreshPassword ( PasswordModel model ) {
            if ( !ModelState.IsValid || UserHelper.PasswordCheck( model.Password, model.PasswordRepeat ) ) {
                model.Password = "";
                model.PasswordRepeat = "";
                return View( model );
            }

            var entity = _businessService.GetAsNoTracking( model.UserId );
            entity.Password = Md5Hash.CreateMD5( model.Password ).ToLower();

            if ( !_businessService.Update( entity ) ) {
                ViewBag.DbSuccess = false;
                ArrangeViewBag();
                return View( model );
            }

            TempData["DbSuccess"] = true;
            if ( TempData["ReturnId"] != null ) return RedirectToAction( "Index", new { id = TempData["ReturnId"] } );
            return RedirectToAction( "Index" );
        }

        private AVBS_Client ModelOperations ( ClientModel model, AVBS_Client entity = null ) {

            if ( entity == null ) {
                entity = new AVBS_Client();
            }

            entity.Email = model.Email;
            entity.Identity = model.Identity;
            entity.Name = model.Name;
            entity.Surname = model.Surname;
            entity.Phone = model.Phone;
            entity.Id = model.Id;

            return entity;
            
        }

        public ClientModel ConvertToModel( AVBS_Client entity ) {
            
            return new ClientModel(  ) {
                Email = entity.Email,
                Identity = entity.Identity,
                Name = entity.Name,
                Phone = entity.Phone,
                Id = entity.Id,
                Surname = entity.Surname
            };

        }


        public JsonResult GetList ( JQueryDataTableParam param, int locationid = 0 ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                //var bus = new ViewBusiness<AVBS_Client>( _loggedUser );
                var allData = _businessService.GetAllQueryable();

                // Search
                if ( !string.IsNullOrEmpty( searchVal ) ) {
                    var searchQuery = searchVal.ToLower();
                    allData = allData.Where( x =>
                                            x.Name.ToLower().Contains( searchQuery ) ||
                                            x.Surname.ToLower().Contains( searchQuery ) ||
                                            x.Email.ToLower().Contains( searchQuery ) ||
                                            x.Phone.ToLower().Contains( searchQuery )
                    );
                }

                // Configure Order
                var orderingFunction = new Func<ClientModel, object>( c => c.Id );
                if ( orderColumn == 1 ) {
                    orderingFunction = ( x =>  x.Name );
                } else if ( orderColumn == 2 ) {
                    orderingFunction = ( x => x.Surname );
                } else if ( orderColumn == 3 ) {
                    orderingFunction = ( x => x.Phone );
                } 

                var selectedData = allData.Select( x=> new ClientModel(  ) { Name = x.Name, Surname = x.Surname, Email = x.Email, Phone = x.Phone, Id = x.Id } );
                // Order and arrange displayed data according to skip, take parameters
                var displayedData = orderDirection == "asc" ? selectedData.OrderBy( orderingFunction ).Skip( param.start ).Take( param.length ).ToList() : selectedData.OrderByDescending( orderingFunction ).Skip( param.start ).Take( param.length ).ToList();

                var totalNumber = allData.Count();

                return
                    Json(
                        new {
                            draw = param.draw,
                            recordsTotal = totalNumber,
                            recordsFiltered = totalNumber,
                            data = displayedData
                        }, JsonRequestBehavior.AllowGet );
            } catch ( Exception e ) {

                return null;
            }
        }

    }
}