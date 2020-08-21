$(function () {
    //dropdownlist valid
    $.validator.addMethod('notEqual', function (value, element, arg) {
        return arg != value;
    }, 'Phải chọn một mục nào đó!');

});

$(document).ready(function () {

    $('#frmCacNN').validate({
        rules: {
            'noidung': {
                required: true
            }
        }
   ,
        messages: {
            'noidung': {
                required: 'Phải nhập nguyên nhân!'
            }
        }
    });

    $('#frmNNghe').validate({
        rules: {
            'tennghanhnghe': {
                required: true
            }
        }
     ,
        messages: {
            'tennghanhnghe': {
                required: 'Phải nhập tên nghành nghề!'
            }
        }
    });


    $('#frmKV').validate({
        rules: {
            'tenkhu': {
                required: true
            }
        }
      ,
        messages: {
            'tenkhu': {
                required: 'Phải nhập tên khu vực!'
            }
        }
    });


    //frmHhEdit
    $('#frmHhEdit').validate({
        rules: {
            'txtsotienhoahong': {
                required: true,
                number: true
            }
            ,
            'salesnm': {
                required: true
            }
            ,
            'tenkhach': {
                required: true
            }
            ,
            'socmnd': {
                required: true
            }
            ,
            'sotien': {
                required: true
            }
        }
      ,
        messages: {
            'txtsotienhoahong': {
                required: 'Phải nhập số tiền!',
                number: 'Phải nhập kiểu số!'
            }
            ,
            'salesnm': {
                required: 'Phải nhập tên sales!'
            }
             ,
            'tenkhach': {
                required: 'Phải nhập tên khách!'
            }
             ,
            'socmnd': {
                required: 'Phải nhập số CMND!'
            }
             ,
            'sotien': {
                required: 'Phải nhập số tiền!'
            }
        }
    });

    $('#frmLoaiT').validate({
        rules: {
            'tenloaitour': {
                required: true
            }
        }
        ,
        messages: {
            'tenloaitour': {
                required: 'Phải nhập tên loại!'
            }
        }
    });

    $('#frmdmdl').validate({
        rules: {
            'Daily': {
                required: true
            }
            ,
            'chinhanh': {
                notEqual: ""
            }
        }
,
        messages: {
            'Daily': {
                required: 'Phải nhập tên đại lý!'
            }
            ,
            'chinhanh': {
                notEqual: "Phải chọn chi nhánh!"
            }
        }
    });


    $('#frmCN').validate({
        rules: {
            'chinhanh1': {
                required: true
                , maxlength: 3
            }
            ,
            'tencn': {
                required: true
            }
            ,
            'maquocgia': {
                notEqual: ""
            }
        }
,
        messages: {
            'chinhanh1': {
                required: 'Phải nhập mã chi nhánh!'
                , maxlength: 'Mã tối đa 3 ký tự!'
            }
             ,
            'tencn': {
                required: 'Phải nhập tên chi nhánh!'
            }
            ,
            'maquocgia': {
                notEqual: "Phải chọn quốc gia!"
            }
        }
    });


    $('#frmquan').validate({
        rules: {
            'tenquan': {
                required: true
            }
            ,
            'maquocgia': {
                notEqual: ""
            }
        }
  ,
        messages: {
            'tenquan': {
                required: 'Phải nhập tên thành phố!'
            }
            ,
            'maquocgia': {
                notEqual: "Phải chọn quốc gia!"
            }
        }
    });


    $('#frmqg').validate({
        rules: {
            'TenNuoc': {
                required: true
            }
        }
    ,
        messages: {
            'TenNuoc': {
                required: 'Phải nhập tên quốc gia!'
            }
        }
    });

    $('#frmdmkh').validate({
        rules: {
            'tengiaodich': {
                required: true
            }
            ,
            'chinhanh': {
                notEqual: ""
            }
             ,
            'quocgia': {
                notEqual: ""
            }
             ,
            'nganhnghe': {
                notEqual: ""
            }
             ,
            'thanhpho': {
                notEqual: ""
            }
        }
    ,
        messages: {
            'tengiaodich': {
                required: 'Phải nhập tên khách hàng!'
            }
            ,
            'chinhanh': {
                notEqual: "Phải chọn chi nhánh của khách hàng!"
            }
            ,
            'quocgia': {
                notEqual: "Phải chọn quốc gia của khách hàng!"
            }
             ,
            'nganhnghe': {
                notEqual: "Phải chọn ngành nghề của khách hàng!"
            }
             ,
            'thanhpho': {
                notEqual: "Phải chọn thành phố của khách hàng!"
            }
        }
    });


    $('#frmThemUser').validate({
        rules: {
            'username': {
                required: true
            }
             ,
            'password': {
                required: true
            }
            ,
            'daily': {
                notEqual: ""
            }
              ,
            'chinhanh': {
                notEqual: ""
            }
            ,
            'role': {
                notEqual: ""
            }
             ,
            'dienthoai': {
                required: true,
                number: true
            }
        }
     ,
        messages: {
            'username': {
                required: 'Phải nhập tài khoản đăng nhập!'
            }
            ,
            'password': {
                required: 'Phải nhập mật mã!'
            }
           ,
            'daily': {
                notEqual: "Phải chọn đại lý!"
            }
             ,
            'chinhanh': {
                notEqual: "Phải chọn chi nhánh!"
            }
            ,
            'role': {
                notEqual: "Phải chọn role!"
            }
                  ,
            'dienthoai': {
                required: 'Phải nhập số điện thoại!',
                number: 'Phải nhập số!'
            }
        }

    });

    //frmKs
    $('#frmKs').validate({
        rules: {
            'tenkhachsan': {
                required: true
            }
             ,
            'ngaycheckin': {
                required: true
            }
              ,
            'ngaycheckout': {
                required: true
            }
        }
     ,
        messages: {
            'tenkhachsan': {
                required: 'Phải nhập tên khách sạn!'
            }
            ,
            'ngaycheckin': {
                required: 'Phải chọn ngày checkin!'
            }
            ,
            'ngaycheckout': {
                required: 'Phải chọn ngày checkout!'
            }

        }
    });

    $('#frmDmkhachtour').validate({
        rules: {
            'tenkhach': {
                required: true
            }
             ,
            'ngaysinh': {
                required: true
            }
              ,
            'hochieu': {
                required: true
            }
              ,
            'hieuluchochieu': {
                required: true
            }
        }
   ,
        messages: {
            'tenkhach': {
                required: 'Phải nhập tên khách!'
            }
            ,
            'ngaysinh': {
                required: 'Phải chọn ngày sinh!'
            }
            ,
            'hochieu': {
                required: 'Phải nhập hộ chiếu!'
            }
             ,
            'hieuluchochieu': {
                required: 'Phải chọn ngày hết hạn hộ chiếu!'
            }

        }
    });

    //frmTour
    $('#frmTour').validate({
        rules: {
            'chudetour': {
                required: true
            }
             ,
            'batdau': {
                required: true
            }
              ,
            'ketthuc': {
                required: true
            }
              ,
            'diemtq': {
                required: true
            }
              ,
            'sokhachdk': {
                required: true,
                number: true
            }
              ,
            'doanhthudk': {
                required: true,
                number: true

            }
              ,
            'makh': {
                required: true
                , maxlength: 10
            }
               ,
            'tenkh': {
                required: true
            }
            ,'sohopdong': {
                number:true
            }
             ,
            'nguontour': {
                notEqual: ""
            }
            ,
            'loaitourid': {
                required: true
            }
        }
      ,
        messages: {
            'chudetour': {
                required: 'Phải nhập chủ đề tour!'
            }
            ,
            'batdau': {
                required: 'Phải chọn ngày bắt đầu!'
            }
            ,
            'ketthuc': {
                required: 'Phải chọn ngày kết thúc!'
            }
             ,
            'diemtq': {
                required: 'Phải nhập tuyến tham quan!'
            }
             ,
            'sokhachdk': {
                required: 'Phải nhập số khách dự kiến!',
                number: 'Phải nhập kiểu số!'
            }
            ,
            'doanhthudk': {
                required: 'Phải nhập doanh thu dự kiến!',
                number: 'Phải nhập kiểu số!'
            }
             ,
            'makh': {
                required: 'Phải nhập mã khách hàng!'
                 , maxlength: 'Mã khách hàng tối đa 10 ký tự!'
            }
               ,
            'tenkh': {
                required: 'Phải nhập tên khách hàng!'
            }
             ,'sohopdong': {
                 number: "Số hợp đồng kiểu số"
             }
             ,
             'nguontour': {
                 notEqual: "Phải chọn nguồn tour"
             }
             ,
             'loaitourid': {
                 required: "Phải chọn loại tour"
             }
        }
    });

    $('#frmMainMenu').validate({
        rules: {
            'areaname': {
                required: true
            }
             ,
            'areacss': {
                required: true
            }
        }
       ,
        messages: {
            'areaname': {
                required: 'Phải nhập tên khu!'
            },
            'areacss': {
                required: 'Phải nhập css!'
            }

        }
    });

    $('#frmSubMenu').validate({
        rules: {
            'menunm': {
                required: true
            }
            ,
            'actionnm': {
                required: true
            }
            ,
            'controllernm': {
                required: true
            }
             ,
            'areaid': {
                notEqual: ""
            }
             ,
            'Role': {
                notEqual: ""
            }
        }
       ,
        messages: {
            'menunm': {
                required: 'Phải nhập tên menu!'
            }
            ,
            'actionnm': {
                required: "Phải nhập actionnm!"
            }
            ,
            'controllernm': {
                required: "Phải nhập controllernm!"
            }
            ,
            'areaid': {
                notEqual: "Phải chọn khu!"
            }
            ,
            'Role': {
                notEqual: "Phải chọn loại người dùng!"
            }
        }
    });
    //validator by class
    //$.validator.addClassRules('ctycss', {
    //    required:true
    //});

});