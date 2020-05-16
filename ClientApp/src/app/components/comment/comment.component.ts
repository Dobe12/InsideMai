import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {Comment} from "../../core/models/comment";
import {ToastrService} from "ngx-toastr";
import {UserReactionsService} from "../../core/services/user-reactions.service";
import {AuthService} from "../../core/auth/auth.service";
import {ConfirmDeleteModalComponent} from "../confirm-delete-modal/confirm-delete-modal.component";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})

export class CommentComponent implements OnInit {

  @Input() comment: Comment;
  @Output() onDeleteComment = new EventEmitter();

  constructor(private userReactionsService: UserReactionsService,
              private toastr: ToastrService,
              public authService: AuthService,
              private dialog: MatDialog
  ) { }

  ngOnInit(): void {
  }
// Потом вынести все в общий сервис!!!
  switchLike(comment: Comment) {
    if (!comment.likedByCurrentUser) {
      this.addLike(comment);
      this.userReactionsService.addLikeToComment(comment.id)
        .subscribe(() => {
          this.toastr.success('Вам понравилася комментарий');
        }, error =>  this.removeLike(comment));
    } else {
      this.removeLike(comment);
      this.userReactionsService.removeLikeFromComment(comment.id).subscribe(() => {
      }, error => this.addLike(comment));
    }
  }

  private addLike(comment: Comment) {
    this.comment.likedByCurrentUser = true;
    this.comment.likesCount++;
  }

  private removeLike(comment: Comment) {
    this.comment.likedByCurrentUser = false;
    this.comment.likesCount--;
  }

  deleteComment(commentId: number) {
    const dialogRef = this.dialog.open(ConfirmDeleteModalComponent, {
      position: {top: '200px'},
      autoFocus: true,
      data: {
        id: commentId,
        title: 'Вы действительно хотите удалить комментарий ?'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if  (result) {
        this.onDeleteComment.emit(result);
      }
    });
  }
}
