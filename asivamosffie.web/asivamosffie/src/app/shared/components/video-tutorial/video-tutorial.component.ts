import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-video-tutorial',
  templateUrl: './video-tutorial.component.html',
  styleUrls: ['./video-tutorial.component.scss']
})
export class VideoTutorialComponent implements OnInit {
  infoVideo: any;
  videoTutorial: any;
  videoTutoriales = [];

  constructor(private router: Router, private route: ActivatedRoute, private commonService: CommonService) {
    // if (this.router.getCurrentNavigation().extras.state)
    //   this.infoVideo = this.router.getCurrentNavigation().extras.state.infoVideo;
    // console.log(this.infoVideo);
  }

  ngOnInit(): void {
    this.getVideoTutoriales();
  }

  routerId() {
    this.route.params.subscribe((params: Params) => {  
      if (params.id === '1') {
        this.videoTutorial = this.videoTutoriales[0];
      }
      else if (params.id === '2') {
        this.videoTutorial = this.videoTutoriales[1];
      }
      else if (params.id === '3') {
        this.videoTutorial = this.videoTutoriales[2];
      }
      else if (params.id === '4') {
        this.videoTutorial = this.videoTutoriales[3];
      }
      
      // if (params.id === '1') {
      //   this.infoVideo = {
      //     nombre: 'Video Dirección Tecnica',
      //     descripcion: 'assets/videos/1.VideoDireccionTecnica.MP4'
      //   }
      // }
      // else if (params.id === '2') {
      //   this.infoVideo = {
      //     nombre: 'Video Dirección Financiera',
      //     descripcion: 'assets/videos/2.VideoDireccionFinanciera.MP4'
      //   }
      // }
      // else if (params.id === '3') {
      //   this.infoVideo = {
      //     nombre: 'Video Dirección Juridica',
      //     descripcion: 'assets/videos/3.VideoDireccionJuridica.MP4'
      //   }
      // }
      // else if (params.id === '4') {
      //   this.infoVideo = {
      //     nombre: 'Video Fiduciaria',
      //     descripcion: 'assets/videos/4.VideoFiduciaria.MP4.MP4'
      //   }
      // }
    });
  }

  getVideoTutoriales() {
    this.commonService.getVideos().subscribe(response => {
      this.videoTutoriales = response;
      this.routerId();
    });
  }

  video(nombre: string) {
      const video = this.videoTutoriales.find(video => video.nombre === nombre);
      return video.descripcion;
  }
}
