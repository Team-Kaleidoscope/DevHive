import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-post-attachment',
  templateUrl: './post-attachment.component.html',
  styleUrls: ['./post-attachment.component.css']
})
export class PostAttachmentComponent implements OnInit {
  @Input() paramURL: string;
  public isImage = false;
  public showFull = false;
  public fileName: string;
  public fileType: string;

  constructor()
  { }

  ngOnInit(): void {
    this.isImage = this.paramURL.includes('image') && !this.paramURL.endsWith('pdf');
    this.fileType = this.isImage ? 'img' : 'raw';
    this.fileName = this.paramURL.match('(?<=\/)(?:.(?!\/))+$')?.pop() ?? 'Attachment';
  }

  toggleShowFull(): void {
    this.showFull = !this.showFull;
  }
}
