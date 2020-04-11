import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {Comment} from "../../core/models/comment";
import {AuthService} from "../../core/auth/auth.service";

@Component({
  selector: 'app-new-comment-form',
  templateUrl: './new-comment-form.component.html',
  styleUrls: ['./new-comment-form.component.scss']
})
export class NewCommentFormComponent implements OnInit {
  @Output() addedComment = new EventEmitter();
  constructor(public auth: AuthService) { }

  ngOnInit(): void {
  }
  addComment(text: string) {
    const newComment = new Comment();
    newComment.content = text;

    this.addedComment.emit(newComment);
  }

}
