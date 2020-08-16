import { TimeSpanWrapper } from '../_models/time-span-wrapper';

export class TimeSpanWrapperMapper {
    public static convert(data): TimeSpanWrapper {
        if (data === null || data === undefined) {
            return new TimeSpanWrapper();
        }

        let timeSpanWrapper = new TimeSpanWrapper();
        timeSpanWrapper.ticks = data.ticks;

        return timeSpanWrapper;
    }
}
