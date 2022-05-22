import $ from "jquery";

/**
 * Secret question field of the sign-up form.
 * @param props
 * @returns {JSX.Element}
 */
function SecretQuestionsField({props}) {

    //function makes sure that the user picked a security question.
    const handleBlur = ()=>{
        let secretQuestionField = $("#secret-questions");
        if (secretQuestionField.val() === "0"){
            secretQuestionField.removeClass("border-success");
            secretQuestionField.addClass("border-danger");
            props.setConfirm(false);
        }
        else{
            secretQuestionField.removeClass("border-danger");
            secretQuestionField.addClass("border-success");
            props.setConfirm(true);
        }
    }

    return (
        <div className="row mb-3">
            <label htmlFor="exampleEmail" className="col-4 col-form-label">
                Security question:
                {props.renderRequired && <span className = "required-field"> *</span>}
            </label>
                <div className="col-auto">
                    <select className="form-select" id="secret-questions" onBlur={handleBlur} required>
                        <option defaultValue value="0">Choose security question</option>
                        <option value="1">What was your elementary's school name?</option>
                        <option value="2">What is / was your dog's name?</option>
                        <option value="3">What is your favorite sport?</option>
                        <option value="4">In what city did you grow up in?</option>
                        <option value="5">What was your nick-name as a child?</option>
                        <option value="6">What was your favorite teacher's name?</option>
                    </select>
                </div>
            {props.children}
        </div>
    )
}

export default SecretQuestionsField;