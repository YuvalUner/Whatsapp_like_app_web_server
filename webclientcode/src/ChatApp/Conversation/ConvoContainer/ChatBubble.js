import {Component} from "react";

/**
 * class of the chat-bubble that holds all types of message.
 */
class ChatBubble extends Component {

    //function puts the time sent of each message in chat bubble.
    parseTime = () => {
        let dateParse =  Date.parse(this.props.time);
        let date = new Date(dateParse);
        let minutes = date.getMinutes().toString().padStart(2, "0");
        let hours = date.getHours().toString().padStart(2, "0");
        return hours + ":" + minutes;
    }

    //function in charge of determining side of message depending on if the message is from sender or not.
    determineSide = () => {
        let side = this.props.sender ? "right" : "left"
        return "chat-bubble chat-bubble-" + side +" chat-bubble-" + this.props.type + "-" + side;
    }

    // function determines the margin of the message if its from sender or not.
    determineMargin = () => {
        let side = this.props.sender ? "offset-md-9" : ""
        return "col-md-3 " + side;
    }

    // function determines the type of content being sent, and adds it accordingly.
    createContent = () => {
        if (this.props.type === "text") {
            return this.props.content;
        } else if (this.props.type === "img") {
            return (<img className="chat-img" src={this.props.content}/>)
        } else if (this.props.type === "video") {
            return (<video width="100%" height="100%" controls>
                <source src={this.props.content}/>
            </video>);
        }
        else{
            return(<audio controls>
                <source src={this.props.content} type="audio/webm"/>
            </audio>);
        }
    }

    /**
     * render function that creates the bubble by determining the margin, side, time and content being sent.
     * @returns {JSX.Element}
     */
    render() {
        return (
            <>
                <div className="row no-gutters">
                    <div className={this.determineMargin()}>
                        <div className={this.determineSide()}>
                            <span>
                                {this.createContent()}
                            </span>
                            <span className="very-small-text float-end">
                                {this.parseTime()}
                            </span>
                        </div>
                    </div>
                </div>
            </>
        )
    }
}

export default ChatBubble;