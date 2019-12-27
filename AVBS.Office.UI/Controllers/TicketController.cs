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
using AVBS.Office.UI.Models.Ticket;
using AVBS.Office.UI.Models.User;

namespace AVBS.Office.UI.Controllers {
    [Authentication]
    [AuthorizeRoles( Role.YONETICI_AVUKAT, Role.AVUKAT )]
    public class TicketController : OfficeController< AVBS_Ticket > {
        
        public override void ArrangeViewBag ( int id = 0 ) {

            var clientBus = new BaseOfficeBusiness<AVBS_Client>( _loggedUser  );
            ViewBag.ClientList = clientBus.GetAllQueryable().Select( x=> new { Id = x.Id, Name = x.Name + " " + x.Surname} ).ToList();

            var personnelBus = new BaseOfficeBusiness<AVBS_User>( _loggedUser );
            ViewBag.PersonnelList = personnelBus.GetAllQueryable().Select( x => new { Id = x.Id, Name = x.Name + " " + x.Surname } ).ToList();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create ( TicketModel model ) {
            if ( !ModelState.IsValid ) { 
                ViewBag.DbSuccess = false;
                ArrangeViewBag();
                return View( model );
            }

            var entity = ModelOperations( model );
            entity.QuestionDate = DateTime.Now;
            entity.IsAssigned = true;

            if ( !_businessService.Add( entity ) ) {
                ViewBag.DbSuccess = false;
                ArrangeViewBag();
                return View( model );
            }

            TempData["DbSuccess"] = true;

            if ( TempData["ReturnId"] != null ) return RedirectToAction( "Index", new { id = TempData["ReturnId"] } );
            return RedirectToAction( "Index" );
        }

        public override ActionResult Edit ( int id ) {
            ArrangeViewBag( id );
            EditSpecifications( id );
            AVBS_Ticket entity = _businessService.GetAsNoTracking( id );
            var model = ConvertToModel( entity );

            return View( model );
        }
        public override ActionResult EditPartial ( int id ) {
            ArrangeViewBag( id );
            EditSpecifications( id );
            AVBS_Ticket entity = _businessService.GetAsNoTracking( id );
            var model = ConvertToModel( entity );

            return PartialView( model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit ( TicketModel model ) {
            if ( !ModelState.IsValid ) {
                ViewBag.DbSuccess = false;
                ArrangeViewBag( model.Id );
                EditSpecifications( model.Id );
                return View( model );
            }

            var entity = _businessService.Get( model.Id );

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
        

        private AVBS_Ticket ModelOperations ( TicketModel model, AVBS_Ticket entity = null ) {

            if ( entity == null ) {
                entity = new AVBS_Ticket();
            }

            entity.QuestionHeader = model.QuestionHeader;
            entity.QuestionText = model.QuestionText;
            entity.AssignedUserId = model.AssignedUserId;
            entity.ClientId = model.ClientId;
            entity.Id = model.Id;

            return entity;
        }

        public TicketModel ConvertToModel ( AVBS_Ticket entity ) {
            
            return new TicketModel(  ) {
                QuestionHeader = entity.QuestionHeader,
                QuestionText = entity.QuestionText,
                QuestionDate = entity.QuestionDate,
                ClientId = entity.ClientId,
                AssignedUserId = entity.AssignedUserId,
                AnswerDate = entity.AnswerDate,
                Id = entity.Id,
                IsAnswered = entity.IsAnswered,
                IsAssigned = entity.IsAssigned,
                AnswerText = entity.AnswerText,
                AnsweredUserId = entity.AnsweredUserId
            };
        }

        public ActionResult AssignPartial ( int id ) {

            var entity = _businessService.Get( id );

            var personnelBus = new BaseOfficeBusiness<AVBS_User>( _loggedUser );
            ViewBag.PersonnelList = personnelBus.GetAllQueryable().Select( x => new { Id = x.Id, Name = x.Name + " " + x.Surname } ).ToList();
            TicketModel model = new TicketModel() {
               Id = id,
               AssignedUserId = entity.AssignedUserId ?? 0
            };
            return PartialView( "AssignPartial", model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPartial ( TicketModel model ) {
            if ( model.AssignedUserId == null ) {
                return View( model );
            }

            var entity = _businessService.GetAsNoTracking( model.Id );
            entity.AssignedUserId = model.AssignedUserId;
            entity.IsAssigned = true;

            if ( !_businessService.Update( entity ) ) {
                ViewBag.DbSuccess = false;
                ArrangeViewBag();
                return View( model );
            }

            TempData["DbSuccess"] = true;
            if ( TempData["ReturnId"] != null ) return RedirectToAction( "Index", new { id = TempData["ReturnId"] } );
            return RedirectToAction( "Index" );
        }

        public ActionResult AnswerPartial ( int id ) {
            var entity = _businessService.Get( id );
            TicketModel model = new TicketModel() { Id = id, AnswerText = entity.AnswerText };
            return PartialView( "AnswerPartial", model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnswerPartial ( TicketModel model ) {
            if ( string.IsNullOrEmpty(model.AnswerText) ) {
                return View( model );
            }

            var entity = _businessService.GetAsNoTracking( model.Id );
            entity.AnsweredUserId = _loggedUser.Id;
            entity.IsAnswered = true;
            entity.AnswerDate = DateTime.Now;
            entity.AnswerText = model.AnswerText;

            if ( !_businessService.Update( entity ) ) {
                ViewBag.DbSuccess = false;
                ArrangeViewBag();
                return View( model );
            }

            TempData["DbSuccess"] = true;
            if ( TempData["ReturnId"] != null ) return RedirectToAction( "Index", new { id = TempData["ReturnId"] } );
            return RedirectToAction( "Index" );
        }


        public JsonResult GetList ( JQueryDataTableParam param, int locationid = 0 ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                var allData = _businessService.GetAllQueryable();

                // Search
                if ( !string.IsNullOrEmpty( searchVal ) ) {
                    var searchQuery = searchVal.ToLower();
                    allData = allData.Where( x =>
                                            x.QuestionHeader.ToLower().Contains( searchQuery ) ||
                                            x.AVBS_Client.Name.ToLower().Contains( searchQuery ) ||
                                            x.AVBS_Client.Surname.ToLower().Contains( searchQuery )
                    );
                }

                // Configure Order
                var orderingFunction = new Func<TicketModel, object>( c => c.Id );
                if ( orderColumn == 1 ) {
                    orderingFunction = ( x =>  x.QuestionDate );
                } else if ( orderColumn == 2 ) {
                    orderingFunction = ( x => x.ClientName );
                } else if ( orderColumn == 3 ) {
                    orderingFunction = ( x => x.IsAssigned );
                } else if ( orderColumn == 4 ) {
                    orderingFunction = ( x => x.AssignedUserName );
                } else if ( orderColumn == 5 ) {
                    orderingFunction = ( x => x.IsAnswered );
                } else if ( orderColumn == 6 ) {
                    orderingFunction = ( x => x.AnsweredUserName );
                }

                var selectedData = allData.Select( x=> new TicketModel(  ) { QuestionHeader = x.QuestionHeader, QuestionDate = x.QuestionDate, IsAnswered = x.IsAnswered, IsAssigned = x.IsAssigned, ClientName = x.AVBS_Client.Name + " " + x.AVBS_Client.Surname, AssignedUserName  = (x.IsAssigned ) ? x.AVBS_AssignedUser.Name + " " + x.AVBS_AssignedUser.Surname : "-" , AnswerDate  = x.AnswerDate, AnsweredUserName = ( x.IsAnswered ) ? x.AVBS_AnsweredUser.Name + " " + x.AVBS_AnsweredUser.Surname : "-", Id = x.Id} );
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