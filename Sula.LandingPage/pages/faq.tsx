import * as React from 'react';
import Layout from '../components/Layout';
import { NextPage } from 'next';
import useTranslation from 'next-translate/useTranslation';
import { useEffect, useState } from 'react';
import {
  Accordion,
  AccordionItem,
  AccordionItemButton,
  AccordionItemHeading,
  AccordionItemPanel,
} from 'react-accessible-accordion';

interface FaqItem {
  question: string;
  answer: string;
}

const FaqPage: NextPage = () => {
  const { t, lang } = useTranslation('faq');

  const [faqs, setFaqs] = useState<FaqItem[]>([]);

  useEffect(() => {
    let index = 0;
    let faqItem: FaqItem = t(index.toLocaleString(), null, { returnObjects: true });
    const faqItemArray: FaqItem[] = [];

    while (typeof faqItem === 'object') {
      faqItemArray.push(faqItem);

      setFaqs(faqItemArray);

      index++;
      faqItem = t(index.toLocaleString(), null, { returnObjects: true });
    }
  }, [lang]);

  return (
    <Layout title="Sula — Faq">
      <h1 className="text-4xl lg:text-5xl font-bold mt-20 opacity-90">Frequently asked questions</h1>

      <Accordion allowMultipleExpanded={true} allowZeroExpanded={true} className="py-10">
        {faqs.map((faq, index) => (
          <AccordionItem key={index} uuid={faq.question.replace(/\s/g, '-')}>
            <AccordionItemHeading>
              <AccordionItemButton className=" faq-gradient text-2xl font-bold mb-8 mt-8">
                {faq.question}
              </AccordionItemButton>
            </AccordionItemHeading>
            <AccordionItemPanel>
              <p className="opacity-80 animate text-white">{faq.answer}</p>
            </AccordionItemPanel>
          </AccordionItem>
        ))}
      </Accordion>
    </Layout>
  );
};

export default FaqPage;
