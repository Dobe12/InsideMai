import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewCommentFormComponent } from './new-comment-form.component';

describe('NewCommentFormComponent', () => {
  let component: NewCommentFormComponent;
  let fixture: ComponentFixture<NewCommentFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewCommentFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewCommentFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
