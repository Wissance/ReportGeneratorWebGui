/* This is not working 
$("#reports-params-files-list").change(function () {
    $("#get-report-param-file-btn").prop('disabled', false);
    // console.log("Selected parameters file was changed");
    var paramsFiles = reportsManagerModel.parametersFiles;
    var selectedValues = $("#reports-params-files-list").val();  // multiselect ? why ?
    var selectedValue = selectedValues != null && selectedValues.length > 0 ? selectedValues[0] : "";
    $("#selected-4gen-param-file").text(selectedValue);
    for (var i = 0; i < paramsFiles.length; i++) {
        var pf = paramsFiles[i];
        if (pf.parametersFileName === selectedValue){
            console.log("updating interface for file: " + pf.parametersFileName);
            // implement update ...
            // header to procedure name
            $("#param-file-name-title").text(pf.parametersFileName);
            $("#form-selected-param-file").val(pf.parametersFileName);
            // display name
            var displayName = pf.displayName;
            if (displayName != null) {
                $("#param-file-display-name").val(displayName);
            }
            else {
                $("#param-file-display-name").val("");
            }

            var description = pf.description;
            if (description != null) {
                $("#param-file-description").val(description);
            }
            else {
                $("#param-file-description").val("");
            }
            $("#params-file-content").val(pf.fileContent.join("\n"));
        }
    }
    if ($("#form-selected-param-file").val() !== "" && $("#form-selected-template-file").val() !== "")
        $("#generate-form-submit-btn").prop('disabled', false);
});

$("#reports-templates-files-list").change(function() {
        $("#get-report-template-file-btn").prop('disabled', false);
    var selectedValues = $("#reports-templates-files-list").val();  // multiselect ? why ?
    var selectedValue = selectedValues != null && selectedValues.length > 0 ? selectedValues[0] : "";
    $("#selected-4gen-template-file").text(selectedValue);
    $("#form-selected-template-file").val(selectedValue);
    if ($("#form-selected-param-file").val() !== "" && $("#form-selected-template-file").val() !== "")
        $("#generate-form-submit-btn").prop('disabled', false);
});

*/