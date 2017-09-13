
$(document).ready(function () {

    $("input[name='selectedRoles']:checkbox:checked").each(function () {
        var str = $(this).parent().text();
        var roleName = str.replace(/\s/g, '').substr(5);
        if (roleName.toString() === "SuperAdmin") {
            $(this).parent().parent().parent().siblings().addClass("hidden");

        }
    });

    $("span.role").on("click", function () {
        if ($(this).children("input[name='selectedRoles']").prop("checked")) {

            $(this).children("input[name='selectedRoles']").prop("checked", false);
            $(this).removeClass("btn-info").addClass("btn-default");

           $(this).parent().parent().siblings().removeClass("hidden");

            $(this).parent().siblings().children().find("input[name = 'selectedActionCategories']").prop("checked", false);
            $(this).parent().siblings().find("span.action-cateogry").removeClass("btn-success").addClass("btn-default");


            $(this).parent().siblings().children().find("input[name = 'selectedActions']").prop("checked", false);
            $(this).parent().siblings().find("span.controller-action").removeClass("btn-primary").addClass("btn-default");
        } else {

            $(this).children("input[name='selectedRoles']").prop("checked", true);
            $(this).removeClass("btn-default").addClass("btn-info");

            var str = $(this).text();
            var roleName = str.replace(/\s/g, '').substr(5);
            if (roleName.toString() === "SuperAdmin") {
                $(this).parent().parent().siblings().addClass("hidden");

                $(this).parent().parent().siblings().children().find("input").prop("checked", false);
                $(this).parent().parent().siblings().children().find("span").removeClass("btn-primary btn-success btn-info").addClass("btn-default");
            }

            $(this).parent().siblings().children().find("input[name = 'selectedActionCategories']").prop("checked", true);
            $(this).parent().siblings().find("span.action-cateogry").removeClass("btn-default").addClass("btn-success");

            $(this).parent().siblings().children().find("input[name = 'selectedActions']").prop("checked", true);
            $(this).parent().siblings().find("span.controller-action").removeClass("btn-default").addClass("btn-primary");
        }

    });


    $("input[name = 'selectedRoles']").on("change", function () {
        if ($(this).prop('checked')) {
            $(this).prop("checked", false);
            $(this).parent().removeClass("btn-info").addClass("btn-default");


            $(this).parent().parent().parent().siblings().removeClass("hidden");


            $(this).parent().parent().siblings().children().find("input[name = 'selectedActionCategories']").prop('checked', false);
            $(this).parent().parent().siblings().children().find("span.action-cateogry").removeClass("btn-success").addClass("btn-default");


            $(this).parent().parent().siblings().children().find("input[name = 'selectedActions']").prop('checked', false);
            $(this).parent().parent().siblings().children().find("span.controller-action").removeClass("btn-primary").addClass("btn-default");
        } else {
            $(this).prop("checked", true);
            $(this).parent().removeClass("btn-default").addClass("btn-info");

            var str = $(this).parent().text();
            var roleName = str.replace(/\s/g, '').substr(5);
            if (roleName.toString() === "SuperAdmin") {
                $(this).parent().parent().parent().siblings().addClass("hidden");
                $(this).parent().parent().siblings().children().find("input").prop("checked", false);
                $(this).parent().parent().siblings().children().find("span").removeClass("btn-primary btn-success btn-info").addClass("btn-default");
            }

            $(this).parent().parent().siblings().children().find("input[name = 'selectedActionCategories']").prop('checked', true);
            $(this).parent().parent().siblings().children().find("span.action-cateogry").removeClass("btn-default").addClass("btn-success");

            $(this).parent().parent().siblings().children().find("input[name = 'selectedActions']").prop('checked', true);
            $(this).parent().parent().siblings().children().find("span.controller-action").removeClass("btn-default").addClass("btn-primary");
        }


    });



    $("span.action-cateogry").on("click", function () {
        if ($(this).children("input:checkbox").prop("checked")) {

            var totalChecked = $(this).parent().siblings().children().find("input[name = 'selectedActionCategories']:checkbox:checked").length;
            if (totalChecked <= 0) {
                $(this).parent().parent().siblings().children().find("input[name = 'selectedRoles']").prop("checked", false);
                $(this).parent().parent().siblings().children().removeClass("btn-info").addClass("btn-default");
            }

            $(this).removeClass("btn-success").addClass("btn-default");
            $(this).siblings().removeClass("btn-primary").addClass("btn-default");

            $(this).children("input:checkbox").prop("checked", false);
            $(this).siblings().children("input[name = 'selectedActions']").prop("checked", false);
        } else {
            $(this).parent().parent().siblings().children().find("input[name = 'selectedRoles']").prop("checked", true);
            $(this).parent().parent().siblings().children().removeClass("btn-default").addClass("btn-info");

            $(this).removeClass("btn-default").addClass("btn-success");
            $(this).siblings().removeClass("btn-default").addClass("btn-primary");

            $(this).children("input:checkbox").prop("checked", true);
            $(this).siblings().children("input[name = 'selectedActions']").prop("checked", true);
        }

    });


    $("span.controller-action").on("click", function () {
        if ($(this).children("input:checkbox").prop("checked")) {

            $(this).removeClass("btn-primary").addClass("btn-default");
            $(this).children("input:checkbox").prop("checked", false);

            var totalChecked = $(this).siblings().children("input[name = 'selectedActions']:checkbox:checked").length;
            if (totalChecked <= 0) {

                var totalActionCategoryChecked = $(this).parent().siblings().children().find("input[name = 'selectedActionCategories']:checkbox:checked").length;
                if (totalActionCategoryChecked <= 0) {

                    $(this).parent().parent().siblings().children().find("input[name = 'selectedRoles']").prop("checked", false);
                    $(this).parent().parent().siblings().children().removeClass("btn-info").addClass("btn-default");
                }

                $(this).siblings("span.action-cateogry").removeClass("btn-success").addClass("btn-default");
                $(this).siblings().children("input[name = 'selectedActionCategories']").prop("checked", false);
            }
        } else {
            $(this).removeClass("btn-default").addClass("btn-primary");
            $(this).siblings("span.action-cateogry").removeClass("btn-default").addClass("btn-success");

            $(this).parent().parent().siblings().children().find("input[name = 'selectedRoles']").prop("checked", true);
            $(this).parent().parent().siblings().children().removeClass("btn-default").addClass("btn-info");

            $(this).children("input:checkbox").prop("checked", true);
            $(this).siblings().children("input[name = 'selectedActionCategories']").prop("checked", true);
        }

    });



    $("input[name = 'selectedActionCategories']").on("change", function () {
        if ($(this).prop("checked")) {
            $(this).prop("checked", false);
            $(this).parent().removeClass("btn-success").addClass("btn-default");

            var totalChecked = $(this).parent().parent().siblings().children().find("input[name = 'selectedActionCategories']:checkbox:checked").length;
            if (totalChecked <= 0) {
                $(this).parent().parent().parent().siblings().children().find("input[name = 'selectedRoles']").prop("checked", false);
                $(this).parent().parent().parent().siblings().children().removeClass("btn-info").addClass("btn-default");
            }

            $(this).parent().siblings().removeClass("btn-primary").addClass("btn-default");
            $(this).parent().siblings().children("input[name = 'selectedActions']").prop("checked", false);
        } else {
            $(this).prop("checked", true);
            $(this).parent().removeClass("btn-default").addClass("btn-success");

            $(this).parent().parent().parent().siblings().children().find("input[name = 'selectedRoles']").prop("checked", true);
            $(this).parent().parent().parent().siblings().children().removeClass("btn-default").addClass("btn-info");

            $(this).parent().siblings().removeClass("btn-default").addClass("btn-primary");
            $(this).parent().siblings().children("input[name = 'selectedActions']").prop("checked", true);
        }
    });

    $("input[name = 'selectedActions']").on("change", function () {
        if ($(this).prop("checked")) {

            $(this).prop("checked", false);
            $(this).parent().removeClass("btn-primary").addClass("btn-default");

            var totalChecked = $(this).parent().siblings().children("input[name = 'selectedActions']:checkbox:checked").length;
            if (totalChecked <= 0) {
                $(this).parent().siblings().children("input[name = 'selectedActionCategories']").prop("checked", false);
                $(this).parent().siblings("span.action-cateogry").removeClass("btn-success").addClass("btn-default");

                var totalActionCategoryChecked = $(this).parent().parent().siblings().children().find("input[name = 'selectedActionCategories']:checkbox:checked").length;
                if (totalActionCategoryChecked <= 0) {

                    $(this).parent().parent().parent().siblings().children().find("input[name = 'selectedRoles']").prop("checked", false);
                    $(this).parent().parent().parent().siblings().children().removeClass("btn-info").addClass("btn-default");
                }

            }
        } else {
            $(this).prop("checked", true);
            $(this).parent().removeClass("btn-default").addClass("btn-primary");

            $(this).parent().parent().parent().siblings().children().find("input[name = 'selectedRoles']").prop("checked", true);
            $(this).parent().parent().parent().siblings().children().removeClass("btn-default").addClass("btn-info");

            $(this).parent().siblings().children("input[name = 'selectedActionCategories']").prop("checked", true);
            $(this).parent().siblings("span.action-cateogry").removeClass("btn-default").addClass("btn-success");
        }
    });

});