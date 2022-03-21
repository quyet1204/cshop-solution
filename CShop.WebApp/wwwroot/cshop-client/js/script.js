  $(document).ready(function(){
    $(".mob-tbtn").click(function(){
      $(this).next().toggle(300);
      $(this).toggleClass("active");
    });

    $(".box-category > ul > li > a").click(function(){
      $("ul.level").hide();
      $(".box-category > ul > li > a").removeClass('active');
      $(this).next().toggle(300);
      $(this).toggleClass("active");
    });

  });