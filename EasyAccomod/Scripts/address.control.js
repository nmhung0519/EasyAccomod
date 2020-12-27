$(".city-filter").ready(function () {
    $(".city-filter").change(function (e) {
        $(".district-filter").empty();
        $(".district-filter").append("<option value='0'>Chọn Quận/Huyện</option>");
        $(".ward-filter").empty();
        $(".ward-filter").append("<option value='0'>Chọn Phường/Xã</option>");
        $(".ward-filter").prop("disabled", true);
        if ($(e.target).val() == 0) {
            $(".district-filter").prop("disabled", true);
            return;
        }
        cityChange($(e.target).val());
    })
})

$(".district-filter").ready(function (e) {
    $(".district-filter").change(function (e) {
        $(".ward-filter").empty();
        $(".ward-filter").append("<option value='0'>Chọn Phường/Xã</option>");
        if ($(e.target).val() == 0) {
            $(".ward-filter").prop("disabled", true);
            return;
        }
        districtChange($(e.target).val());
    })
})

async function selectCity(cityId) {
    $(".city-filter").val(cityId);
    return cityChange(cityId);
}

async function selectDistrict(districtId) {
    $(".district-filter").val(districtId);
    return districtChange(districtId);
}

function selectWard(wardId) {
    return $(".ward-filter").val(wardId);
}

async function loadCity() {
    $(".city-filter").empty();
    $(".city-filter").append("<option value='0'>Chọn Tỉnh/Thành phố</option>");
    $(".district-filter").empty();
    $(".district-filter").append("<option value='0'>Chọn Quận/Huyện</option>");
    $(".ward-filter").prop("disabled", true);
    $(".ward-filter").empty();
    $(".ward-filter").append("<option value='0'>Chọn Phường/Xã</option>");
    $(".ward-filter").prop("disabled", true);
    return $.ajax({
        url: "/Address/CityList",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $(".city-filter").append("<option value='" + data[i].id + "'>" + data[i].name + "</option>");
            }
        }
    })
}

function cityChange(cityId) {
    $(".district-filter").prop("disabled", false);
    return $.ajax({
        url: "/Address/DistrictList",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        data: { id: cityId },
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $(".district-filter").append("<option value='" + data[i].id + "'>" + data[i].name + "</option>");
            }
        }
    })
}

function districtChange(districtId) {
    $(".ward-filter").prop("disabled", false);
    return $.ajax({
        url: "/Address/WardList",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        data: { id: districtId },
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $(".ward-filter").append("<option value='" + data[i].id + "'>" + data[i].name + "</option>");
            }
        }
    })
}

