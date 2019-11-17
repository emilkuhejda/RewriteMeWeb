import { Pipe, PipeTransform } from '@angular/core';
import { SubscriptionOperation } from '../_enums/subscription-operation';

@Pipe({
    name: 'subscriptionOperation'
})
export class SubscriptionOperationPipe implements PipeTransform {
    transform(value: SubscriptionOperation): any {
        if (value == SubscriptionOperation.Add)
            return 'Add';

        if (value == SubscriptionOperation.Remove)
            return 'Remove';

        return '';
    }
}
