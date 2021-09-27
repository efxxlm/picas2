import { Component, OnInit } from '@angular/core';
import { AutenticacionService } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  showFiller = false;
  responsiveOpen = false;
  videoTutoriales = [];

  constructor(
    private autenticacionService: AutenticacionService,
    private router: Router,
    private commonService: CommonService
  ) {}

  ngOnInit(): void {
    this.getVideoTutoriales();
  }

  getVideoTutoriales() {
    this.commonService.getVideos().subscribe(response => {
      this.videoTutoriales = response;
    });
  }

  logout() {
    this.autenticacionService.logout();
  }
}
