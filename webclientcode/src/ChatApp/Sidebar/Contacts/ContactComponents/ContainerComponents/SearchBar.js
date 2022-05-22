import {useRef} from "react";

/**
 * A search bar for the contacts.
 * @param props
 * @returns {JSX.Element}
 */
function SearchBar({props}) {

    const val = useRef();

    return (
        <div className="input-group pe-0 ps-0">
            <i className="bi bi-search input-group-text"/>
            <input ref={val} onChange={() => props.update(val.current.value)} type="search"
                   className="form-control" placeholder="Search contacts"/>
        </div>
    )
}

export default SearchBar;