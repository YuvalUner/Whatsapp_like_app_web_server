/**
 * The descriptor underneath the secret question.
 * @returns {JSX.Element}
 */
function SecretQuestionDescriptor(){
    return (
        <div className="col-auto">
            <span id="secret-questions" className="form-text"> Secret question will be asked if the user forgot his password or username.</span>
        </div>
    )
}

export default SecretQuestionDescriptor;