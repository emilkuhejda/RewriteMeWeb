import { SubscriptionOperation } from '../_enums/subscription-operation';

export class UserSubscription {
    id: string;
    userId: string;
    applicationId: string;
    time: string;
    operation: SubscriptionOperation;
    dateCreated: Date;
}
