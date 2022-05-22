import {Component} from "react";
import {Button, Modal} from "react-bootstrap";
import $ from "jquery";
import RegisteredUser from "../../../../../../Users/RegisteredUser";

/**
 * A modal for changing text related data.
 */
class ChangeTextDataModal extends Component {

    constructor(props) {
        super(props);
        this.state = {
            valid: false,
            description: null
        }
    }

    handleClose = (e) => {
        e.stopPropagation();
        this.props.hide(false);
    }

    /**
     * Updates data, depending on that data's rules.
     * @param e
     */
    handleSubmit = async (e) => {
        e.preventDefault();
        let val = $("#new-desc-input").val();
        let errText = $("#desc-choose-error");
        if (this.props.textData === "description") {
            if (val.match("^[\\w\\s]{1,100}$")) {
                await RegisteredUser.changeDescription(this.props.username, val);
                this.props.connection.invoke("changedDescription", this.props.username);
                this.handleClose(e);
            } else {
                errText.show();
            }
        } else if (this.props.textData === "nickname") {
            if (val.match("^[\\w\\s]{1,10}$")) {
                await RegisteredUser.updateNickname(this.props.username, val);
                this.handleClose(e);
                this.props.updateNickname();
            } else {
                errText.show();
            }
        }
    }

    async componentDidMount() {
        this.setState({
            valid: true,
        });
    }

    render() {
        return (
            <>
                {this.state.valid &&
                    <Modal
                        show={this.props.show}
                        size="sm"
                        aria-labelledby="change-description-modal"
                        centered
                    >
                        <Modal.Header closeButton onClick={this.handleClose}>
                            <Modal.Title id="change-description-modal">
                                Change description
                            </Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <div>
                                <span>
                                {this.props.description}
                                </span>
                            </div>
                            <form onSubmit={async (e) => await this.handleSubmit(e)} className="m-3">
                                <label className="col-label" htmlFor="new-desc-input">New {this.props.textData}:</label>
                                <div className="col-auto input-group mt-2">
                                    <span className="input-group-text"><i className="bi bi-person-fill"/></span>
                                    <input type="text" placeholder={this.props.textData} className="form-control"
                                           id="new-desc-input"/>
                                    <div className="error-text" id="desc-choose-error">
                                        Error: {this.props.textData} must be
                                        {this.props.textData === "description" && " 1-100 "}
                                        {this.props.textData === "nickname" && " 1-10 "}
                                        characters
                                        long and contain no special characters
                                    </div>
                                </div>
                            </form>
                        </Modal.Body>
                        <Modal.Footer>
                            <Button className="btn-danger" onClick={this.handleClose}>Close</Button>
                            <Button onClick={async (e) => this.handleSubmit(e)}>Change {this.props.textData}</Button>
                        </Modal.Footer>
                    </Modal>
                }
            </>
        )
    }
}

export default ChangeTextDataModal;