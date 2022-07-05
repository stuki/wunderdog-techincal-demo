import React, { ReactElement } from 'react';
import Loader from 'react-loader-spinner';

export function LoadingView(): ReactElement {
  return (
    <div className="w-full h-full flex justify-center items-center">
      <Loader type="ThreeDots" color="#99bb7c" />
    </div>
  );
}
