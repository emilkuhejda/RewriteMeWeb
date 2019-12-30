import { SubscriptionOperation } from '../_enums/subscription-operation';

export class UserSubscription {
    id: string;
    userId: string;
    applicationId: string;
    time: any;
    operation: SubscriptionOperation;
    dateCreated: Date;
}
