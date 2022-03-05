import React, { createContext, useState, ComponentType } from "react";

const AuthContext = createContext({});

export const AuthProvider: React.FC = ({ children }) => {
  const [auth, setAuth] = useState({});
  return (
    <AuthContext.Provider value={{ auth, setAuth }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;

// const Wrapper = <P extends WrapperProps>(WrappedComponent: React.ComponentType<P>): React.FunctionComponent<P> => {
//   const Wrapper = (props: P) => {
//       return (
//         specialProp ? (
//           <div style={{background: 'pink'}}>
//             <WrappedComponent {...props.style} {...props} />
//           </div> )
//         : <WrappedComponent {...props.style} {...props} />
//   };
// };
