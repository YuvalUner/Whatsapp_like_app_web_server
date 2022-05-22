import $ from "jquery"
import PendingUser from "../../../Users/PendingUser";
import RegisteredUser from "../../../Users/RegisteredUser";

/**
 * Button for re-sending code to the user's email.
 * @param props
 * @returns {JSX.Element}
 */
function ResendCodeButton({props}) {

    // Re-send the code, and then count down 60 seconds before re-enabling this.
    const reSend = async () => {
        if (props.fromSignup) {
            await PendingUser.renewCode(props.username);
        }
        else{
            await RegisteredUser.generateVerCode(props.username);
        }
        let button = $("#re-send-button");
        button.append("<br> <div class='small-text' id='text-area'></div>")
        let textArea = $("#text-area");
        let totalSeconds = 60;
        button.addClass("btn-secondary");
        button.removeClass("btn-primary");
        button.prop("disabled", true);
        for (let i = 0; i <= totalSeconds; i++) {
            setTimeout(() => {
                textArea.text((60 - i) + "s");
            }, i * 1000);
        }
        setTimeout(() => {
            button.prop("disabled", false);
            button.addClass("btn-primary");
            button.removeClass("btn-secondary");
            button.find("br:last").remove();
            textArea.remove();
        }, totalSeconds * 1000)
    }

    return (
        <button type="button" id="re-send-button" onClick={reSend}
                className="btn btn-primary form-text text-wrap small-text">
            Re-send code <br/>
            <i className="bi bi-arrow-repeat"/>
        </button>
    )
}

export default ResendCodeButton;