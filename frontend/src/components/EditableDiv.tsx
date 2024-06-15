import React, {useEffect, useRef} from "react";

export const EditableDiv:React.FC<{
    onKeyDown: (event: React.KeyboardEvent) => void,
    clazzName: string,
    content: string | null,
    setContent: (content: string | null) => void
}> = ({clazzName, content, setContent, onKeyDown}) => {

    const editableRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (content && editableRef.current) {
            editableRef.current.textContent = content;
        } else if (editableRef.current) {
            editableRef.current.textContent = "";
        }
    }, [content]);

    const handleChange = (e: React.ChangeEvent<HTMLDivElement>) => {
        if (editableRef.current) {
            editableRef.current.textContent = e.target.textContent;
            setContent(e.target.textContent);
        }
    };

    const handleKeyDown = (event: React.KeyboardEvent) => {
        onKeyDown(event);
    }

    return (
        <div ref={editableRef}
             contentEditable={true}
             className={clazzName}
             onInput={handleChange}
             onKeyDown={handleKeyDown}/>
    )
}