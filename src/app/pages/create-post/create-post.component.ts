import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss' ]
})
export class CreatePostComponent implements OnInit {
  markdown: any;


  constructor() { }

  ngOnInit(): void {
  }


}
