import $ from "jquery";

/**
 * Secret answer field in signup form.
 * @param props
 * @returns {JSX.Element}
 */
function SecretQuestionAnswerField({props}){

    //function checks that that secret answer isn't empty, if empty prints error.
    const handleBlur = ()=>{
        let secretAnswerField = $("#secret-answer");
        let text = $("#secret-answer-error");
        if (secretAnswerField.val() === ""){
            secretAnswerField.removeClass("border-success");
            secretAnswerField.addClass("border-danger");
            text.text("Error: field can not be empty")
            text.show();
            props.setConfirm(false);
        }
        //else condition checks that answer isn't empty, and colors border in green.
        else{
            secretAnswerField.removeClass("border-danger");
            secretAnswerField.addClass("border-success");
            text.hide();
            props.setConfirm(true);
        }
    }

    return(
        <div className="row mb-3">
            <label htmlFor="secret-answer" className="col-2 me-2 col-form-label">Answer:
                {props.renderRequired && <span className = "required-field"> *</span>}
            </label>
            <div className="col-8">
                <input type="text" id="secret-answer" className="form-control" onBlur={handleBlur} required/>
                <div className="error-text" id="secret-answer-error"/>
            </div>
        </div>
    )
}

export default SecretQuestionAnswerField;