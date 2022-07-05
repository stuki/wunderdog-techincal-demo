import { FieldAttributes } from 'formik';
import React, { ReactElement } from 'react';

export function InputField({ field, ...props }: FieldAttributes<any>): ReactElement {
  return (
    <input {...field} {...props} className="form-gradient w-full px-4 py-4 mt-2 text-base text-white rounded-lg" />
  );
}

export function SelectField({ field, ...props }: FieldAttributes<any>): ReactElement {
  return (
    <select {...field} {...props} className="form-gradient w-full px-4 py-4 mt-2 text-base text-white rounded-lg" />
  );
}
