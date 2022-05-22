import pdf from "../../../Policies/privacy_policy.pdf"

/**
 * Privacy policy check box in signup form.
 * @returns {JSX.Element}
 */
function PrivacyPolicyField() {
    return (
        <div className="form-check row mb-3">
            <div className="form-check">
                <input className="form-check-input" type="checkbox" value="" id="privacy-policy-radio-check"/>
                <label className="form-check-label" htmlFor="privacy-policy-radio-check">
                    I have read and accepted the
                    <a target="_blank" href={pdf}> privacy policy</a>
                </label>
                <div className="error-text" id="invalid-pp">
                    You must accept the privacy policy to sign-up.
                </div>
            </div>
        </div>
    )
}

export default PrivacyPolicyField;