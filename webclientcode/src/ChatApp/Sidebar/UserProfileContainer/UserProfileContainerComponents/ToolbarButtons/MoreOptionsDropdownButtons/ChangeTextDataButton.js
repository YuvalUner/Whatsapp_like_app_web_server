import {Component} from "react";
import ChangeTextDataModal from "./ChangeTextDataModal";
import RegisteredUser from "../../../../../../Users/RegisteredUser";

/**
 * A button for opening a modal for changing text related data of the user.
 */
class ChangeTextDataButton extends Component {

    constructor(props) {
        super(props);
        this.state = {show: false, description: null}
    }

    /**
     * Generates a description, depending on the type of data being changed.
     * @returns {JSX.Element}
     */
    generateDescription = async () => {
        if (this.props.textData === "description") {
            return (
                <>
                    Current description: <br/><br/>
                    {await RegisteredUser.getDescription(this.props.username)}
                    <br/>
                    <hr/>
                </>
            )
        } else if (this.props.textData === "nickname") {
            return (
                <>
                    Current nickname: <br/><br/>
                    {await RegisteredUser.getNickname(this.props.username)}
                    <br/>
                    <hr/>
                </>
            )
        }
    }

    /**
     * Updates whether the related modal should open or close.
     * @param val true or false.
     */
    handleClick = async (val)=>{
        this.setState({
            show: val,
            description: await this.generateDescription()
        });
    }



    render() {
        return (
            <div className="d-grid hover-pointer mb-2" onClick={()=>this.handleClick(true)}>
                <span>Change {this.props.textData}</span>
                <ChangeTextDataModal username={this.props.username} show={this.state.show} hide={this.handleClick}
                textData={this.props.textData} updateNickname={this.props.updateNickname}
                description={this.state.description} connection={this.props.connection}/>
            </div>
        )
    }
}

export default ChangeTextDataButton;