import React from "react";
import { Check, Info, X } from "react-feather";
import { AlertTemplateProps } from "react-alert";

export const AlertTemplate: React.ComponentType<AlertTemplateProps> = ({
  options,
  message,
}) => {
  return (
    <div id="Alert" className={options.type} style={{ marginBottom: 20 }}>
      {options.type === "success" ? (
        <Check />
      ) : options.type === "error" ? (
        <X />
      ) : (
        <Info />
      )}
      <span>{message}</span>
    </div>
  );
};
export default AlertTemplate;
