import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {Post} from "../../core/models/post";
import {ToastrService} from "ngx-toastr";
import {UserReactionsService} from "../../core/services/user-reactions.service";
import {User} from "../../core/models/user";
import {PostsService} from "../../core/services/posts.service";
import {AuthService} from "../../core/auth/auth.service";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ChangePasswordFormComponent} from "../change-password-form/change-password-form.component";
import {ConfirmDeleteModalComponent} from "../confirm-delete-modal/confirm-delete-modal.component";

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {
  @Input() post: Post;
  @Output() onDeletePost = new EventEmitter();
  constructor(private userReactionsService: UserReactionsService,
              private toastr: ToastrService,
              public authService: AuthService,
              private dialog: MatDialog) { }

  ngOnInit(): void {
    if (this.post.isAnonymous) {
      this.post.author.fullName = "Анонимно";
      this.post.author.userPic = 'assets/images/defaultUserPic.png';
    }
  }

  switchLike(post: Post) {
    if (!post.likedByCurrentUser) {
      this.addLike(post);
      this.userReactionsService.addLikeToPost(post.id)
        .subscribe(() => {
          this.toastr.success('Вам понравилась публикация');
        }, error =>  this.removeLike(post));
    } else {
      this.removeLike(post);
      this.userReactionsService.removeLikeFromPost(post.id).subscribe(() => {
      }, error => this.addLike(post));
    }
  }

  switchFav(post: Post) {
    if (!post.addedToFavByCurrentUser) {
      this.post.addedToFavByCurrentUser = true;
      this.userReactionsService.addToFav(post.id)
        .subscribe(() => {
          this.toastr.success('Публикация добавлена в избанное');
        }, error =>   this.post.addedToFavByCurrentUser = false);
    } else {
      this.post.addedToFavByCurrentUser = false;
      this.userReactionsService.removeFromFav(post.id).subscribe(() => {
      }, error => this.post.addedToFavByCurrentUser = true);
    }
  }

  private addLike(post: Post) {
    this.post.likedByCurrentUser = true;
    this.post.likesCount++;
  }

  private removeLike(post: Post) {
    this.post.likedByCurrentUser = false;
    this.post.likesCount--;
  }


  deletePost(postId: number) {
    const dialogRef = this.dialog.open(ConfirmDeleteModalComponent, {
      position: {top: '200px'},
      autoFocus: true,
      data: {
        id: postId,
        title: 'Вы действительно хотите удалить публикацию ?'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if  (result) {
        this.onDeletePost.emit(result);
      }
    });

  }
}
