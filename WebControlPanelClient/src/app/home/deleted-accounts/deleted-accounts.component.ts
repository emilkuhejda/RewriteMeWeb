import { Component, OnInit } from '@angular/core';
import { DeletedAccountService } from 'src/app/_services/deleted-account.service';

@Component({
    selector: 'app-deleted-accounts',
    templateUrl: './deleted-accounts.component.html',
    styleUrls: ['./deleted-accounts.component.css']
})
export class DeletedAccountsComponent implements OnInit {
    constructor(private deletedAccountService: DeletedAccountService) { }

    ngOnInit() {
        this.deletedAccountService.getAll().subscribe(accounts => {
            console.log(accounts);
        });

        // this.deletedAccountService.delete("f9f01e95-ba8e-4cd4-93b1-347d5f213d54").subscribe();
    }
}
