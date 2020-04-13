import { Component, OnInit, Input } from '@angular/core';
import {Post} from "../../core/models/post";
import {ToastrService} from "ngx-toastr";
import {UserReactionsService} from "../../core/services/user-reactions.service";

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {
@Input() post: Post;
  constructor(private userReactionsService: UserReactionsService,
              private toastr: ToastrService) { }

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
}
