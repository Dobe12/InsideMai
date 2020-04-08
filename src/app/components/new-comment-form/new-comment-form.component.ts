import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {Comment} from "../../core/models/comment";

@Component({
  selector: 'app-new-comment-form',
  templateUrl: './new-comment-form.component.html',
  styleUrls: ['./new-comment-form.component.scss']
})
export class NewCommentFormComponent implements OnInit {
  @Output() addedComment = new EventEmitter();
  constructor() { }

  ngOnInit(): void {
  }
  addComment(text: string) {
    const newComment = new Comment();
    newComment.content = text;

    this.addedComment.emit(newComment);
  }

}
