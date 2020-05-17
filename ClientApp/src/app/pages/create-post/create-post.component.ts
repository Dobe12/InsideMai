import {Component, ElementRef, OnInit} from '@angular/core';
import {removeElementFromHtml} from "@angular/material/schematics/ng-update/upgrade-rules/hammer-gestures-v9/remove-element-from-html";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {ToastrService} from "ngx-toastr";
import {PostsService} from "../../core/services/posts.service";
import {Router} from "@angular/router";


@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss' ]
})
export class CreatePostComponent implements OnInit {
  markdown: any;
  createPostForm: FormGroup;
  name: any;
  postTypes = [
    {id: 'question', value: 2, name: 'Вопрос'},
    {id: 'advert', value: 4, name: 'Объявление'},
    {id: 'event', value: 5, name: 'Событие'},
    {id: 'article', value: 3, name: 'Статья'},
    ];

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private postsService: PostsService,
    private router: Router) {
    this.createPostForm = this.fb.group(
      {
        title: [''],
        isAnonymous: new FormControl(false),
        content: ['', Validators.required],
        type: new FormControl(2)
      });
  }

  ngOnInit(): void {
  }

  createPost() {

    if (this.createPostForm.invalid) {
      this.toastr.error('Нельзя создать пустой пост');
      return;
    }

    const post = this.createPostForm.value;
    this.postsService.create(post).subscribe(
      data => {
        this.toastr.success('Пост успешно создан');
        this.router.navigate(['/all/all']);
      });
  }

}

