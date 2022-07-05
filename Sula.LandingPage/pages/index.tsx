import * as React from 'react';
import Layout from '../components/Layout';
import { NextPage } from 'next';
import Hero from '../components/Hero';
import CallToAction from '../components/CallToAction';

const IndexPage: NextPage = () => {
  return (
    <Layout title="Sula — Remote IoT Monitoring">
      <Hero />
      <CallToAction />
    </Layout>
  );
};

export default IndexPage;
