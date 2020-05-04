import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {Comment} from "../../core/models/comment";
import {AuthService} from "../../core/auth/auth.service";

@Component({
  selector: 'app-new-comment-form',
  templateUrl: './new-comment-form.component.html',
  styleUrls: ['./new-comment-form.component.scss']
})
export class NewCommentFormComponent implements OnInit {

  commentText: string;

  @Output() addedComment = new EventEmitter();
  constructor(public auth: AuthService) { }

  ngOnInit(): void {
  }
  addComment() {
    const newComment = new Comment();
    newComment.content = this.commentText;
    this.addedComment.emit(newComment);
    this.commentText = '';
  }

}
