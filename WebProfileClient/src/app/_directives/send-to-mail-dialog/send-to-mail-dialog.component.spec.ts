import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SendToMailDialogComponent } from './send-to-mail-dialog.component';

describe('SendToMailDialogComponent', () => {
  let component: SendToMailDialogComponent;
  let fixture: ComponentFixture<SendToMailDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SendToMailDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SendToMailDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
