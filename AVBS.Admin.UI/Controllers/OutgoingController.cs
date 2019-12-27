using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Admin.UI.Controllers.Base;
using AVBS.Business.Abstract;
using AVBS.Entity.Concrete;
using AVBS.Infrastructure;
using AVBS.View.Concrete.SelectList;
using AVBS.View.Concrete.Table;
using AVBS.Admin.UI.App_Data;
using AVBS.Business.Concrete;

namespace AVBS.Admin.UI.Controllers {
    [Authentication]
    public class OutgoingController : IController< AVBS_Outgoing, BaseBusiness<AVBS_Outgoing>> {

        public PartialViewResult IndexPartial ( int outgoingid ) {
            ViewBag.OutgoingId = outgoingid;
            return PartialView( "Index" );
        }

        public override void ArrangeIndexViewBag () { 
             
        }

        public override void ArrangeViewBag ( int id = 0 ) {
            var subOutgoingTypeService = new ViewBusiness<AVBS_SubOutgoingTypeSelectListView>();
            ViewBag.SubOutgoingTypeList = subOutgoingTypeService.GetAllQueryable( ).Select( x => new { x.Id, x.Name } ).ToList(  );

        }

        // TODO model bazlı kontrol yapılacak
        public override void EditSpecifications ( int id = 0 ) { 
        }

        public override void CreateSpecifications ( int id = 0 ) { 
        }

        public override void ModelOperations ( ref AVBS_Outgoing model ) {

        }
         

        public JsonResult GetList ( JQueryDataTableParam param, int outgoingSourceTypeId = 0, DateTime? date = null ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );


            try {
                var outgoingBusiness = new ViewBusiness<AVBS_OutgoingTableView>();
                var allData = outgoingBusiness.GetAllQueryable();

                if ( outgoingSourceTypeId > 0 ) allData = allData.Where( x=>x.OutgoingSourceTypeId == outgoingSourceTypeId );
                if ( date.HasValue ) {

                    var startDate = date.Value;
                    var endDate = date.Value.AddMonths( 1 ).AddDays( -1 );
                    allData = allData.Where( x => startDate <= x.Date && endDate >= x.Date );
                }

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
                var orderingFunction = new Func<AVBS_OutgoingTableView, object>( c => c.Id );
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
                var displayedData = orderDirection == "desc" ? allData.OrderBy( orderingFunction ).Skip( param.start ).Take( param.length ).ToList() : allData.OrderByDescending( orderingFunction ).Skip( param.start ).Take( param.length ).ToList();

                var totalNumber = allData.Count();
                decimal totalOutgoing = 0;

                if (totalNumber > 0 ) totalOutgoing = allData.Sum( x => x.Amount );

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