import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeletedAccountsComponent } from './deleted-accounts.component';

describe('DeletedAccountsComponent', () => {
  let component: DeletedAccountsComponent;
  let fixture: ComponentFixture<DeletedAccountsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeletedAccountsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeletedAccountsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
