﻿using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Admin.UI.Controllers.Base;
using AVBS.Admin.UI.Models;
using AVBS.Business.Abstract;
using AVBS.Entity.Concrete;
using AVBS.Entity.References;
using AVBS.Enum;
using AVBS.Infrastructure;
using AVBS.View.Concrete.SelectList;
using AVBS.View.Concrete.Table;
using AVBS.Admin.UI.App_Data;
using AVBS.Business.Concrete;

namespace AVBS.Admin.UI.Controllers {
    [Authentication]
    public class CashBookOutgoingController : IController< AVBS_Outgoing, BaseBusiness<AVBS_Outgoing>> {

        public PartialViewResult IndexPartial ( int cashbookid ) {
            ViewBag.CashBookId = cashbookid;
            return PartialView( "Index" );
        }

        public override void ArrangeIndexViewBag () { 
             
        }

        public override void ArrangeViewBag ( int id = 0 ) {
            var subOutgoingTypeService = new ViewBusiness<AVBS_SubOutgoingTypeSelectListView>();
            ViewBag.SubOutgoingTypeList = subOutgoingTypeService.GetAllQueryable( ).ToList(  ).Select( x => new { x.Id, x.Name } ); ;

        }

        // TODO model bazlı kontrol yapılacak
        public override void EditSpecifications ( int id = 0 ) {
            var cashbookoutgoing = _businessService.Get( id ); 

            ViewBag.CashBook = cashbookoutgoing.AVBS_CashBook;
        }

        public override void CreateSpecifications ( int id = 0 ) {
            var cashbookService = new BaseBusiness<AVBS_CashBook>();
            var cashbook = cashbookService.Get( id );

            ViewBag.CashBook = cashbook;
        }

        public override void ModelOperations ( ref AVBS_Outgoing model ) {
            model.OutgoingSourceTypeId = OutgoingSourceType.Cash.GetHashCode( );
        }
         

        public JsonResult GetList (  JQueryDataTableParam param, int cashbookid = 0 ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                var cashBookBusiness = new ViewBusiness<AVBS_CashBookOutgoingTableView>();

                var allData = cashBookBusiness.GetAllQueryable( );

                if ( cashbookid > 0 ) allData = allData.Where( x => x.CashBookId == cashbookid );

                // Search
                if ( !string.IsNullOrEmpty( searchVal ) ) {
                    var searchQuery = searchVal.ToLower();
                    allData = allData.Where( x =>
                                            x.OutgoingTypeName.ToLower().Contains( searchQuery ) ||
                                            x.SubOutgoingTypeName.ToLower().Contains( searchQuery ) ||
                                            x.Description.ToLower().Contains( searchQuery ) 
                    );
                }

                // Configure Order
                var orderingFunction = new Func<AVBS_CashBookOutgoingTableView, object>( c => c.Id );
                if ( orderColumn == 0 ) {
                    orderingFunction = ( x => x.Date );
                } else if ( orderColumn == 1 ) {
                    orderingFunction = ( x => x.OutgoingTypeId );
                } else if ( orderColumn == 2 ) {
                    orderingFunction = ( x => x.SubOutgoingTypeId );
                } else if ( orderColumn == 3 ) {
                    orderingFunction = ( x => x.Amount );
                } 

                // Order and arrange displayed data according to skip, take parameters
                var displayedData = orderDirection == "asc" ? allData.OrderBy( orderingFunction ).Skip( param.start ).Take( param.length ).ToList() : allData.OrderByDescending( orderingFunction ).Skip( param.start ).Take( param.length ).ToList();

                var totalNumber = allData.Count();
                decimal totalOutgoing = 0;

                if ( totalNumber > 0 ) totalOutgoing = allData.Sum( x => x.Amount );

                return
                    Json(
                        new {
                            draw = param.draw,
                            recordsTotal = totalNumber,
                            recordsFiltered = totalNumber,
                            data = displayedData,
                            totalOutgoing = totalOutgoing
                        }, JsonRequestBehavior.AllowGet );
            } catch ( Exception e ) {

                return null;
            }

        }


    }
}