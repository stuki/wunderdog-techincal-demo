import React, { ReactElement, useEffect, useRef, useState } from 'react';
import { DataType, Limit, Operator, SensorData } from '../../interfaces';
import { Group } from '@visx/group';
import { coerceNumber, scaleLinear, scaleUtc } from '@visx/scale';
import { timeFormat } from 'd3-time-format';
import { AxisBottom, AxisLeft } from '@visx/axis';
import { ParentSize } from '@visx/responsive';
import { NumberValue, ScaleLinear, ScaleTime } from 'd3';
import { AreaClosed, LinePath, Polygon } from '@visx/shape';
import { PatternLines } from '@visx/pattern';
import { Drag } from '@visx/drag';

function getMinMax(vals: Array<number | { valueOf(): number }>) {
  const numericVals = vals.map(coerceNumber);
  return [Math.min(...numericVals) - 2, Math.max(...numericVals) + 5];
}

function LineGraph(props: {
  dataType: DataType;
  data: SensorData[];
  width: number;
  height: number;
  filterAmount?: number;
  limits?: Limit[];
}): ReactElement {
  const [filteredDataset] = useState(
    props.filterAmount ? props.data.reverse().splice(0, props.filterAmount) : props.data
  );

  const margin = { top: 20, left: 20, right: 20, bottom: 20 };

  const isHumidity = props.dataType === DataType.Humidity;

  const xMax = props.width - margin.left - margin.right;
  const yMax = props.height - margin.top - margin.bottom;

  const yMinMax = getMinMax([
    ...filteredDataset.map((data) => data.value),
    ...(props.limits?.map((limit) => limit.value) as number[]),
    -5,
  ]);

  const xTickFormatting = (date: NumberValue | Date) => {
    return timeFormat('%b %d')(date as Date);
  };

  const yTickFormatting = (number: NumberValue) => {
    return Math.floor(number.valueOf()).toString();
  };

  const xScale = scaleUtc({
    range: [margin.left, xMax - margin.right],
    round: true,
    domain: getMinMax(filteredDataset.map((data) => new Date(data.time))),
  });

  const yScale = scaleLinear<number>({
    range: [yMax, 0],
    round: true,
    domain: isHumidity ? [0, 100] : yMinMax,
  });

  const ref = useRef<SVGSVGElement | null>(null);

  return (
    <>
      <svg {...props} ref={ref}>
        <PatternLines id="lines" height={10} width={10} stroke={'red'} strokeWidth={1} orientation={['diagonal']} />
        <Group top={margin.top} left={margin.left}>
          {props.limits?.map((limit) => (
            <LimitBoxes
              {...props}
              limit={limit}
              dataset={filteredDataset}
              yMax={(isHumidity ? [0, 100] : yMinMax)[1]}
              yMin={(isHumidity ? [0, 100] : yMinMax)[0]}
              xScale={xScale}
              yScale={yScale}
              canvasref={ref}
            />
          ))}
          <LinePath
            data={filteredDataset}
            strokeWidth={1}
            strokeOpacity={0.5}
            stroke={'#FFF'}
            x={(data) => xScale(new Date(data.time))}
            y={(data) => yScale(data.value)}
            className="pointer-events-none"
          />
          <AxisLeft
            hideAxisLine
            hideTicks
            top={5}
            left={20}
            scale={yScale}
            stroke={'#FFFFFF'}
            tickStroke={'#FFFFFF'}
            tickLabelProps={() => ({
              fill: '#FFFFFF',
              fontSize: 12,
              fontFamily: 'sans-serif',
              textAnchor: 'end',
            })}
            tickFormat={yTickFormatting}
          />
          <AxisBottom
            hideAxisLine
            hideTicks
            top={yMax}
            left={0}
            scale={xScale}
            stroke={'#FFFFFF'}
            tickStroke={'#FFFFFF'}
            tickLabelProps={() => ({
              fill: '#FFFFFF',
              fontSize: 12,
              fontFamily: 'sans-serif',
              textAnchor: 'middle',
            })}
            tickFormat={xTickFormatting}
          />
        </Group>
      </svg>
    </>
  );
}

export function ResponsiveLineGraph(props: {
  dataType: DataType;
  data: SensorData[];
  filterAmount?: number;
  limits?: Limit[];
}): ReactElement {
  return <ParentSize>{(parent) => <LineGraph {...props} {...parent}></LineGraph>}</ParentSize>;
}

function LimitBoxes(props: {
  limit: Limit;
  dataset: SensorData[];
  xScale: ScaleTime<number, number, never>;
  yScale: ScaleLinear<number, number, never>;
  width: number;
  height: number;
  yMin: number;
  yMax: number;
  canvasref: React.MutableRefObject<SVGSVGElement | null>;
}) {
  const [highlighted, setHighlighted] = useState(false);
  const [locked, setLocked] = useState(false);

  const divider = props.yMax === 100 ? 14 : 10;

  const magicNumber = props.yMax / divider;

  const [value, setValue] = useState(0);
  useEffect(() => {
    console.log('value changes', value);
  }, [value]);

  if (props.limit.operator === Operator.LessThan) {
    return (
      <Group>
        <Drag
          key={props.limit.id}
          width={props.width}
          height={props.height}
          y={props.yScale(props.limit.value)}
          onDragStart={() => setLocked(!locked)}
          onDragEnd={(e) => {
            setLocked(!locked);

            if (e.y) {
              setValue(Math.floor(props.yScale.invert(e.y + e.dy) + magicNumber));
            }
          }}>
          {({ dragStart, dragEnd, dragMove, isDragging, y, dy }) => {
            const newY = Math.floor(props.yScale.invert(y + dy) + magicNumber);

            return (
              <>
                <AreaClosed
                  data={props.dataset}
                  x={(data) => props.xScale(new Date(data.time))}
                  y={props.yScale(newY)}
                  yScale={props.yScale}
                  fill="url('#lines')"
                  className="pointer-events-none"
                />
                <LinePath
                  className="cursor-move"
                  data={props.dataset}
                  strokeWidth={highlighted ? 15 : 5}
                  stroke={isDragging ? 'blue' : 'red'}
                  x={(data) => props.xScale(new Date(data.time))}
                  y={props.yScale(newY)}
                  onMouseEnter={() => {
                    if (!locked) setHighlighted(true);
                  }}
                  onMouseLeave={() => {
                    if (!locked) setHighlighted(false);
                  }}
                  onMouseMove={dragMove}
                  onTouchMove={dragMove}
                  onMouseUp={dragEnd}
                  onTouchEnd={dragEnd}
                  onMouseDown={dragStart}
                  onTouchStart={dragStart}
                />
              </>
            );
          }}
        </Drag>
      </Group>
    );
  }

  if (props.limit.operator === Operator.MoreThan) {
    return (
      <Group>
        <Drag
          key={props.limit.id}
          width={props.width}
          height={props.height}
          y={props.yScale(props.limit.value)}
          onDragStart={() => setLocked(!locked)}
          onDragEnd={(e) => {
            setLocked(!locked);

            if (e.y) {
              setValue(Math.floor(props.yScale.invert(e.y + e.dy) + magicNumber));
            }
          }}>
          {({ dragStart, dragEnd, dragMove, isDragging, y, dy }) => {
            const newY = Math.floor(props.yScale.invert(y + dy) + magicNumber);

            return (
              <>
                <Polygon
                  points={[
                    [props.xScale(new Date(props.dataset[0].time)), props.yScale(props.yMax)],
                    [props.xScale(new Date(props.dataset[0].time)), props.yScale(newY)],
                    [props.xScale(new Date(props.dataset[props.dataset.length - 1].time)), props.yScale(newY)],
                    [props.xScale(new Date(props.dataset[props.dataset.length - 1].time)), props.yScale(props.yMax)],
                  ]}
                  fill="url('#lines')"
                  className="pointer-events-none"
                />
                <LinePath
                  className="cursor-move"
                  data={props.dataset}
                  strokeWidth={highlighted ? 15 : 5}
                  stroke={isDragging ? 'blue' : 'red'}
                  x={(data) => props.xScale(new Date(data.time))}
                  y={props.yScale(newY)}
                  onMouseEnter={() => {
                    if (!locked) setHighlighted(true);
                  }}
                  onMouseLeave={() => {
                    if (!locked) setHighlighted(false);
                  }}
                  onMouseMove={dragMove}
                  onTouchMove={dragMove}
                  onMouseUp={dragEnd}
                  onTouchEnd={dragEnd}
                  onMouseDown={dragStart}
                  onTouchStart={dragStart}
                />
              </>
            );
          }}
        </Drag>
      </Group>
    );
  }

  return null;
}
