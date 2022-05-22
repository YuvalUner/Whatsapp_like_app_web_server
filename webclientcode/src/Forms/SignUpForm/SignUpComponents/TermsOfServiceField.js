import pdf from "../../../Policies/tos.pdf";

/**
 * terms of service check-box in signup form.
 * @returns {JSX.Element}
 */
function TermOfServiceField() {
    return (
        <div className="form-check row mb-3">
            <div className="form-check">
                <input className="form-check-input" type="checkbox" value="" id="tos-radio-check"/>
                <label className="form-check-label" htmlFor="tos-radio-check">
                    I have read and accepted the
                    <a target="_blank" href={pdf}> terms of service</a>
                </label>
                <div className="error-text" id="invalid-tos">
                    You must accept the terms of service to sign-up.
                </div>
            </div>
        </div>
)
}

export default TermOfServiceField;