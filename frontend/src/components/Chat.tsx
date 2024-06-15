import React, {useEffect, useState} from "react";
import {EditableDiv} from "./EditableDiv";
import * as BaseAPI from "../requests/BaseAPI";

export interface IMessage {
  text: string | null,
  isBot: boolean
}

export const Chat = () => {

  const [message, setMessage] = useState<IMessage>();
  const [messageList, setMessageList] = useState<IMessage[]>([]);

  useEffect(() => {
    const botMsg1: IMessage = {
      text: "Вас приветствует служба поддержки компании Росатом.",
      isBot: true
    };
    const myMsg1: IMessage = {
      text: "Хочу задать вопрос, как мне к вам обращаться",
      isBot: false
    };
    const botMsg2: IMessage = {
      text: "Могу ответить только про 1С",
      isBot: true
    };
    const myMsg2: IMessage = {
      text: "Ладно, давай поговорим про 1С",
      isBot: false
    };
    setMessageList([botMsg1, myMsg1, botMsg2, myMsg2]);
  }, []);

  const submit = () => {
    BaseAPI.send(message, (response) => {
      if (response.status === 200) {
        const newList = messageList;
        newList.push(response.data);
        setMessageList(newList);
        setMessage({...message, text: "", isBot: true});
      } else {
        alert("Ошибка отправки сообщения");
      }
    })
  }

  const handleKeyDown = (event: React.KeyboardEvent) => {
    if (event.key === 'Enter') {
      submit();
    }
  }

  return (
      <main>
        <div className="messanger-container">
          <div className="left-block">
            <nav className="messenger-nav">
              <div className="header-logo">
                <img src="/img/atom-svgrepo-com_1_1.svg" className="header-logo-img"/>
                  <h1 className="logo-text">
                    Техподдержка
                  </h1>
              </div>
            </nav>
            <div className="answer-list">
              <div className="question-info">
                <h2>Добро пожаловать</h2>
                <p>
                  Наша служба поддержки готова ответить на ваши вопросы.
                  Создайте новое обращение или отслеживайте ранее созданные.
                </p>
              </div>
              <ul className="menu-list">
                <li className="menu-item new active-item">
                  Новое обращение
                </li>
                <li className="menu-item history">
                  История обращений
                </li>
                <li className="menu-item process">
                  Заявки в обработке
                </li>
              </ul>
            </div>
          </div>
          <div className="right-block">
            <nav className="messenger-nav">
              <div className="header-logo">
                <div className="back-list" id="backlist"/>
                <img src="/img/admin.gif" alt="" className="right-block-avatar"/>
                  <h2 className="logo-text">
                    Бот из 1С
                  </h2>
              </div>
            </nav>
            <div className="container-messages">
              {messageList?.map((msg, index) =>
                  <div className="message" key={index}>
                    <img src={msg.isBot ? "/img/user.svg" : "/img/admin.gif"} alt="" className="avatar-contact-img"/>
                    <div className={`message-text ${msg.isBot ? "my-message": ""}`}>
                      <p className="contact-name">
                        {msg.isBot ? "Бот из 1С" : "Вы"}
                        {/*<span className="time-send-message">12:50</span>*/}
                      </p>
                      <p className="message-text-message">
                        {msg.text}
                      </p>
                      {msg.isBot ?
                          <></>
                          :
                          <p className="message-status">
                            Отправлено
                          </p>
                      }
                    </div>
                  </div>
              )}
              <div className="answer-var">
                <a href="" className="answer-link yes">Вопрос решён</a>
                <a href="" className="answer-link no">Вопрос не решён</a>
                <a href="" className="answer-link request">Уточнить вопрос</a>
              </div>
            </div>
            <div className="block-message-input">
              <EditableDiv
                  onKeyDown={handleKeyDown}
                  clazzName={"input-message"}
                  content={message?.text || ""}
                  setContent={(content) =>
                      setMessage({...message, text: content, isBot: true})}/>
              <button className="btn-send" onClick={submit}/>
            </div>
          </div>
        </div>
      </main>
  )
}