import React, { ReactElement, useEffect, useState } from 'react';
import { Period, SensorMinMaxData } from '../../interfaces';
import { Group } from '@visx/group';
import { BoxPlot } from '@visx/stats';
import { coerceNumber, scaleLinear, scaleUtc } from '@visx/scale';
import { timeFormat } from 'd3-time-format';
import { AxisBottom, AxisLeft } from '@visx/axis';
import { ParentSize } from '@visx/responsive';
import { curveMonotoneX, NumberValue } from 'd3';
import { LinePath } from '@visx/shape';
import { defaultStyles, Tooltip, withTooltip } from '@visx/tooltip';
import { WithTooltipProvidedProps } from '@visx/tooltip/lib/enhancers/withTooltip';

function getMinMax(vals: Array<number | { valueOf(): number }>) {
  const numericVals = vals.map(coerceNumber);
  return [Math.min(...numericVals), Math.max(...numericVals)];
}

interface TooltipData {
  name?: string;
  min?: number;
  mean?: number;
  max?: number;
}

export type StatsPlotProps = {
  width: number;
  height: number;
  data: SensorMinMaxData[];
  period: Period;
};

const BoxGraph = withTooltip<StatsPlotProps, TooltipData>(
  ({
    data,
    period,
    width,
    height,
    tooltipOpen,
    tooltipLeft,
    tooltipTop,
    tooltipData,
    showTooltip,
    hideTooltip,
  }: StatsPlotProps & WithTooltipProvidedProps<TooltipData>) => {
    const [numberOfTicks, setNumberOfTicks] = useState(0);
    const [rounding, setRounding] = useState(5);

    const tickColor = '#c5c4c4';
    const fillColor = '#99bb7c';

    useEffect(() => {
      switch (period) {
        case Period.Week:
          if (data.length < 7) {
            setNumberOfTicks(data.length);
          } else {
            setNumberOfTicks(7);
          }

          setRounding(5);
          break;
        case Period.Month:
          if (data.length < 31) {
            setNumberOfTicks(data.length);
          } else {
            setNumberOfTicks(31);
          }

          setRounding(2);
          break;
        case Period.Year:
          if (data.length < 12) {
            setNumberOfTicks(data.length);
          } else {
            setNumberOfTicks(12);
          }

          setRounding(8);
          break;
      }
    }, [period]);

    const margin = { top: 20, left: 20, right: 20, bottom: 20 };

    const xMax = width - margin.left - margin.right;
    const yMax = height - margin.top - margin.bottom;

    const xTickFormatting = (date: NumberValue | Date) => {
      switch (period) {
        case Period.Week:
          return timeFormat('%a')(date as Date);
        case Period.Month:
          if ((date as Date).getDate() % 2 === 0) {
            return timeFormat('%e')(date as Date);
          } else {
            return '';
          }
        case Period.Year:
          return timeFormat('%b')(date as Date);
      }
    };

    const yTickFormatting = (number: NumberValue) => {
      return Math.floor(number.valueOf()).toString();
    };

    const xScale = scaleUtc({
      range: [margin.left, xMax - margin.right],
      round: true,
      domain: getMinMax(data.map((data) => new Date(data.time))),
    });

    const yScale = scaleLinear<number>({
      range: [yMax, 0],
      round: true,
      domain: [Math.min(...data.map((data) => data.min)) - 1, Math.max(...data.map((data) => data.max)) + 1],
    });

    const mouseOver = (value: number, sensorData: SensorMinMaxData) => {
      return {
        onMouseOver: () => {
          showTooltip({
            tooltipTop: yScale(value),
            tooltipLeft: xScale(new Date(sensorData.time)) + xMax / data.length / 4 + 10,
            tooltipData: {
              ...sensorData,
              name: new Date(sensorData.time).toLocaleDateString(),
            },
          });
        },
        onMouseLeave: () => {
          hideTooltip();
        },
      };
    };

    return (
      <>
        <svg height={height} width={width}>
          <Group top={margin.top} left={margin.left}>
            {data.map((sensorData: SensorMinMaxData, index) => {
              return (
                <g key={index}>
                  <BoxPlot
                    firstQuartile={sensorData.min}
                    thirdQuartile={sensorData.max}
                    rx={rounding}
                    ry={rounding}
                    boxWidth={xMax / data.length / 4}
                    fill={fillColor}
                    valueScale={yScale}
                    left={xScale(new Date(sensorData.time)) + 5}
                    boxProps={mouseOver(sensorData.mean, sensorData)}
                  />
                </g>
              );
            })}

            <LinePath
              data={data}
              strokeWidth={1}
              strokeOpacity={0.5}
              strokeDasharray={4}
              stroke={fillColor}
              curve={curveMonotoneX}
              x={(data) => xScale(new Date(data.time))}
              y={(data) => yScale(data.mean)}
            />
            <AxisLeft
              hideAxisLine
              hideTicks
              top={0}
              left={0}
              numTicks={5}
              scale={yScale}
              stroke={tickColor}
              tickStroke={tickColor}
              tickLabelProps={() => ({
                fill: tickColor,
                fontSize: 12,
                fontFamily: 'sans-serif',
                textAnchor: 'end',
              })}
              tickFormat={yTickFormatting}
            />
            <AxisBottom
              hideAxisLine
              hideTicks
              numTicks={numberOfTicks}
              top={yMax - 10}
              left={5}
              scale={xScale}
              stroke={tickColor}
              tickStroke={tickColor}
              tickLabelProps={() => ({
                fill: tickColor,
                fontSize: 12,
                fontFamily: 'sans-serif',
                textAnchor: 'middle',
              })}
              tickFormat={xTickFormatting}
            />
          </Group>
        </svg>
        {tooltipOpen && tooltipData && (
          <Tooltip
            top={tooltipTop}
            left={tooltipLeft}
            style={{ ...defaultStyles, backgroundColor: '#1e1e1e', color: '#e5e5e5' }}>
            <div>
              <strong>{tooltipData.name}</strong>
            </div>
            <div style={{ marginTop: '5px', fontSize: '12px' }}>
              {tooltipData.max && <div>max: {tooltipData.max}</div>}
              {tooltipData.mean && <div>mean: {tooltipData.mean.toFixed(1)}</div>}
              {tooltipData.min && <div>min: {tooltipData.min}</div>}
            </div>
          </Tooltip>
        )}
      </>
    );
  }
);

export function ResponsiveBoxGraph(props: { data: SensorMinMaxData[]; period: Period }): ReactElement {
  return <ParentSize>{(parent) => <BoxGraph {...props} {...parent}></BoxGraph>}</ParentSize>;
}
