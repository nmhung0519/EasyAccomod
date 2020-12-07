$(".city-filter").ready(function () {
    $.ajax({
        url: "/Address/CityList",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $(".city-filter").append("<option value='" + data[i].id + "'>" + data[i].name + "</option>");
            }
        }
    })
})

$(".city-filter").change(function (e) {
    $(".district-filter").empty();
    $(".district-filter").append("<option value='-1'>Tất cả Quận/Huyện/Thị xã</option>");
    $(".ward-filter").empty();
    $(".ward-filter").append("<option value='-1'>Tất cả Phường/Xã</option>");
    $(".ward-filter").prop("disabled", true);
    if ($(e.target).val() == -1) {
        $(".district-filter").prop("disabled", true);
        return;
    }
    $(".district-filter").prop("disabled", false);
    $.ajax({
        url: "/Address/DistrictList",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        data: {id: $(e.target).val()},
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $(".district-filter").append("<option value='" + data[i].id + "'>" + data[i].name + "</option>");
            }
        }
    })
})

$(".district-filter").change(function (e) {
    $(".ward-filter").empty();
    $(".ward-filter").append("<option value='-1'>Tất cả Phường/Xã</option>");
    if ($(e.target).val() == -1) {
        $(".ward-filter").prop("disabled", true);
        return;
    }
    $(".ward-filter").prop("disabled", false);
    $.ajax({
        url: "/Address/WardList",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        data: { id: $(e.target).val() },
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $(".ward-filter").append("<option value='" + data[i].id + "'>" + data[i].name + "</option>");
            }
        }
    })
})