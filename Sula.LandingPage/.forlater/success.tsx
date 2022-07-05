import * as React from 'react';
import Layout from '../components/Layout';
import { NextPage } from 'next';

const SuccessPage: NextPage = () => {
  return (
    <Layout title="Home | Next.js + TypeScript Example">
      <h1>Hello Next.js 👋</h1>
    </Layout>
  );
};

export default SuccessPage;