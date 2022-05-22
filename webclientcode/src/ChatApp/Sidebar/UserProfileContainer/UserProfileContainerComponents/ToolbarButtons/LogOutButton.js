import CookieHandling from "../../../../../Misc/CookieHandling";
import {useNavigate} from "react-router";
import RegisteredUser from "../../../../../Users/RegisteredUser";

/**
 * A button for logging out the user.
 * @param props
 * @returns {JSX.Element}
 */
function LogOutButton({props}) {

    const nav = useNavigate();

    /**
     * Deletes the cookies related to the user and logs them out.
     */
    const clickHandler = async () => {
        props.setLogIn(false);
        await RegisteredUser.logOut();
        CookieHandling.deleteCookie("rToken");
        nav('/log_in')
    }

    return (
        <button onClick={async () => await clickHandler()} type="button" className="btn no-effect-button btn-lg pe-0"
                data-bs-toggle="tooltip" data-bs-placement="bottom" title="Log out">
            <i className="bi bi-box-arrow-right float-end"/>
        </button>
    )
}

export default LogOutButton;