import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSubscriptionDialogComponent } from './create-subscription-dialog.component';

describe('CreateSubscriptionDialogComponent', () => {
  let component: CreateSubscriptionDialogComponent;
  let fixture: ComponentFixture<CreateSubscriptionDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateSubscriptionDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateSubscriptionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
