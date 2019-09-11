import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateRecognitionStateDialogComponent } from './update-recognition-state-dialog.component';

describe('UpdateRecognitionStateDialogComponent', () => {
  let component: UpdateRecognitionStateDialogComponent;
  let fixture: ComponentFixture<UpdateRecognitionStateDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateRecognitionStateDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateRecognitionStateDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
