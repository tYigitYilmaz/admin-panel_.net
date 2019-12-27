//$.fn.dataTable.ext.buttons.switchCardView = {
//    text: '<i class="fa fa-table fa-sm" aria-hidden="true"></i> ' +
//            '<i class="fa fa-arrows-h fa-sm" aria-hidden="true"></i> ' +
//            '<i class="fa fa-list fa-sm" aria-hidden="true"></i>',
//    action: function (e, dt, node, config) {
//        $("table").toggleClass('cards');
//        $("table thead").toggle();
//        $("table tfoot").toggle();
//    }
//};

$.fn.dataTable.ext.buttons.wholeFloor = {
    text: 'Show Whole Floor',
    action: function (e, dt, node, config) {
        showWholeFloor();
    }
};

// Disable search and ordering by default
$.extend($.fn.dataTable.defaults, {
    processing: true,
    serverSide: true,
    dom: 'it<"bottom"lp>',
    "bInfo": false,
    destroy: true,
    responsive: true,
    rowReorder: {
        selector: ['td:nth-child(0)', 'td:nth-child(3)']
    }
});


$(document).ready(function () {
    $(document).on('click',
        '.remove-element',
        (function (e) {
            e.preventDefault();
            var url = $(this).data("url");

            swal({
                title: "Emin Misiniz?",
                text: "Silme işlemi sonrasında, sildiğiniz nesne geri getirilemez!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Evet, sil",
                cancelButtonText: "İptal et",
                closeOnConfirm: false,
                closeOnCancel: false
            }, function (isConfirm) {
                if (isConfirm) {
                    $.ajax({
                        url: url,
                        type: 'POST',
                        success: function(data) {
                            if (data.error == 1) {
                                swal("Hata", data.message ? data.message : "Silinme işlemi sırasında bir hata oluştu.", "error");
                            } else {
                                swal("Silindi!", "Silme işlemi başarılı.", "success");
                                location.reload();
                            }
                        }
                    });
                } else {
                    swal("Cancelled", "İşlem iptal edildi", "error");
                }

            });
        }));
});

$(document).ready(function () {
    $(document).on('click',
        '.change-status-button',
        (function (e) {
            e.preventDefault();
            var url = $(this).data("url");

            swal({
                title: "Emin Misiniz?",
                text: "Statü Değiştirme İşlemi Yapmak İstediğinize Emin Misiniz!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Evet, değiştir",
                cancelButtonText: "İptal et",
                closeOnConfirm: false,
                closeOnCancel: false
            }, function (isConfirm) {
                if (isConfirm) {
                    $.ajax({
                        url: url,
                        type: 'POST',
                        success: function(data) {
                            if (data.error == 1) {
                                swal("Hata", data.message ? data.message : "Statü değiştirme işlemi sırasında bir hata oluştu.", "error");
                            } else {
                                swal("Statü Değiştirildi!", "Statü değiştirme işlemi başarılı.", "success");
                                location.reload();
                            }
                        }
                    });
                } else {
                    swal("Cancelled", "İşlem iptal edildi", "error");
                }

            });
        }));
});

