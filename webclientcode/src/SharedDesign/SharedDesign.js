import './SharedDesignStyle.css'
import {Component} from "react";
import CookieHandling from "../Misc/CookieHandling";
import Tokens from "../Users/Tokens";

/**
 * The shared design all pages share among them.
 * @param children
 * @returns {JSX.Element}
 */
class SharedDesign extends Component {


    async componentDidMount() {
        let token = CookieHandling.getCookie("rToken");
        if (token){
            let rValue = await Tokens.renewTokens(token, true, true);
            if (rValue){
                this.props.setUser(rValue);
                this.props.setLoggedIn(true);
            }
        }
    }

    render() {
        return (
            <div className="background container-fluid vw-100 vh-100">
                {this.props.children}
            </div>
        )
    }
}

export default SharedDesign;