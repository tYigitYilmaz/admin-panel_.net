using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Business.Concrete;
using AVBS.Entity.Concrete;
using AVBS.Enum;
using AVBS.Infrastructure;
using AVBS.Office.UI.App_Data;
using AVBS.Office.UI.Controllers.Base;
using AVBS.Office.UI.Helper;
using AVBS.Office.UI.Models.User;
using AVBS.View.Concrete.Table;

namespace AVBS.Office.UI.Controllers {
    [Authentication]
    [AuthorizeRoles(Role.YONETICI_AVUKAT)]
    public class PersonnelController : OfficeController< AVBS_User > {

        public override void ArrangeViewBag ( int id = 0 ) {

            var roleList = new BaseViewBusiness<AVBS_RoleSelectListView>(_loggedUser);
            ViewBag.RoleList = roleList.GetAllQueryable().ToList();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create ( UserModel model ) {
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
        public virtual JsonResult CreateWithReturn ( UserModel model ) {
            if ( !ModelState.IsValid ||
                UserHelper.PasswordCheck( model.Password, model.PasswordRepeat ) ||
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

            var returnText = entity.Name + " " + entity.Surname;
            return Json( new { error = 0, value = new { id = entity.Id, text = returnText } } );
        }


        public override ActionResult Edit ( int id ) {
            ArrangeViewBag( id );
            EditSpecifications( id );
            AVBS_User entity = _businessService.GetAsNoTracking( id );
            var model = UserHelper.ConvertToModel( entity );

            return View( model );
        }

        public override ActionResult EditPartial ( int id ) {
            ArrangeViewBag( id );
            EditSpecifications( id );
            AVBS_User entity = _businessService.GetAsNoTracking( id );
            var model = UserHelper.ConvertToModel( entity );

            return PartialView( model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit ( UserModel model ) {
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

        private AVBS_User ModelOperations ( UserModel model, AVBS_User entity = null ) {

            // Save Image
            if ( model.File != null ) {
                var guid = Guid.NewGuid().ToString().Substring( 0, 10 );
                var path = System.IO.Path.GetExtension( model.File.FileName );
                model.ImageUrl = $"/Images/UserImages/{guid}{path}";
                model.File.SaveAs( Server.MapPath( model.ImageUrl ) );
            } else {

                if ( entity != null && !model.ImgRemoved )
                    model.ImageUrl = entity.ImageUrl;
                else model.ImageUrl = "";
            }

            entity = UserHelper.ConvertToEntity( model, entity );
            return entity;
        }




        public JsonResult GetList ( JQueryDataTableParam param, int locationid = 0 ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                var bus = new ViewBusiness<AVBS_PersonnelTableView>( _loggedUser );
                var allData = bus.GetAllQueryable();

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
                var orderingFunction = new Func<UserTableModel, object>( c => c.Id );
                if ( orderColumn == 1 ) {
                    orderingFunction = ( x =>  x.Name );
                } else if ( orderColumn == 2 ) {
                    orderingFunction = ( x => x.Email );
                } else if ( orderColumn == 3 ) {
                    orderingFunction = ( x => x.Phone );
                } else if ( orderColumn == 4 ) {
                    orderingFunction = ( x => x.RoleId );
                }

                var selectedData = allData.Select( x=> new UserTableModel(  ) { Name = x.Name, Surname = x.Surname, Email = x.Email, Phone = x.Phone, RoleId = x.RoleId, Id = x.Id, RoleName = x.RoleName, ImageUrl = x.ImageUrl} );
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